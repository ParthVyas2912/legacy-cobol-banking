#!/usr/bin/env python3
"""
MainframeForge — IMS segment converter for CUSTDB.ADDRESS

Reads the IMS unload output for segment ADDRESS (from DFSURGL0/DFSDDLT0),
decodes each segment occurrence, and writes a pipe-delimited CSV.

Parent segment: CUSTOMER
Segment bytes:  120
Target table:   custdb_address
"""

import sys, csv, decimal, struct
from pathlib import Path

# Import shared decoders (copy from any VSAM converter)
def decode_display(data, enc='cp037'):
    try: return data.decode(enc).rstrip()
    except: return data.decode('latin-1').rstrip()

def unpack_comp3(data, int_d, frac_d):
    digits = []
    for b in data[:-1]: digits += [str((b>>4)&0xF), str(b&0xF)]
    digits.append(str((data[-1]>>4)&0xF))
    sign = data[-1] & 0xF
    raw = ''.join(digits).lstrip('0') or '0'
    if frac_d > 0:
        raw = raw.zfill(frac_d+1)
        raw = raw[:-frac_d]+'.'+raw[-frac_d:]
    r = decimal.Decimal(raw)
    return -r if sign in (0xD,0xB) else r

COLUMNS = [
    'effdate',
    'street',
    'city',
    'state',
    'zip'
]

def decode_segment(raw: bytes) -> dict:
    d = {}
    d['effdate'] = decode_display(raw[0:8])
    d['street'] = decode_display(raw[8:48])
    d['city'] = decode_display(raw[48:73])
    d['state'] = decode_display(raw[73:75])
    d['zip'] = decode_display(raw[75:80])
    return d

def main():
    if len(sys.argv) < 2:
        print(f'Usage: {sys.argv[0]} <ims-unload-file> [output-csv]', file=sys.stderr)
        sys.exit(1)
    input_path = Path(sys.argv[1])
    output_path = Path(sys.argv[2]) if len(sys.argv) > 2 else input_path.with_suffix('.csv')
    # IMS unloads are segment-type-prefixed; filter for ADDRESS
    print(f'TODO: Parse IMS unload format for segment ADDRESS from {input_path}')
    print('IMS unload formats vary by utility (DFSURGL0 vs DFSDDLT0 vs custom).')
    print('Adjust the record reader below to match your site\'s unload format.')

if __name__ == '__main__':
    main()