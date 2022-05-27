import os
import json

# Set up {type: filetype} for further process.
def handle(files, data):
    if files == None:
        data['content'] = data['userUpload']
        del data['userUpload']
        data.update({'type': 'string'})

    else:
        type = files.mimetype
        data.update({'content': files})
        if(type == 'text/plain'):
            data.update({'type': 'text/plain'})

        elif(type == 'application/zip'):
            data.update({'type': 'application/zip'})

        elif(type == 'application/octet-stream'):
            data.update({'type': 'application/octet-stream'})

    return data


# Generate shell command to trigger crypto detector
# A .crypto file be generated and stored inside ./cryptoes directory.
def process(data, base_dir):
    source_dir = base_dir + "\\cryptoes\\" + data['sessionID'] + '.crypto'

    if data['type'] == 'string':
        command = 'python .\crypto-detector-master\scan-for-crypto.py -o cryptoes ' + \
            data['content'] + " " + data['sessionID']
        status = os.system(command)

    elif data['type'] == 'text/plain':
        data['content'] == data['content'].replace('\\', '/')
        command = 'python .\crypto-detector-master\scan-for-crypto.py -o cryptoes ' + \
            data['content'] + ' ' + data['sessionID']
        status = os.system(command)

    elif data['type'] == 'application/zip':
        data['content'] == data['content'].replace('\\', '/')
        command = 'python .\crypto-detector-master\scan-for-crypto.py -o cryptoes ' + \
            data['content'] + ' ' + data['sessionID']
        status = os.system(command)
    
    elif data['type'] == 'application/octet-stream':
        data['content'] == data['content'].replace('\\', '/')
        command = 'python .\crypto-detector-master\scan-for-crypto.py -o cryptoes ' + \
            data['content'] + ' ' + data['sessionID']
        status = os.system(command)

    return readCrypto(source_dir)


# Read the content of generated .crypto file
# Return a list of libraries inside .crypto
def readCrypto(source):
    encryptionLib = []
    with open(source, 'rb') as filename:
        data = json.load(filename)

        for SHA1_checksum in data['crypto_evidence']:
            for index in range(0, len(data['crypto_evidence'][SHA1_checksum]['hits'])):

                encryption = data['crypto_evidence'][SHA1_checksum]['hits'][index]['matched_text']
                evidence = data['crypto_evidence'][SHA1_checksum]['hits'][index]['evidence_type']
                encryption = removeRedundancy(str(encryption))
                if encryption.upper() not in encryptionLib:
                    encryptionLib.append(encryption.upper())

    return encryptionLib


# removing unwanted characters from the evidence string
def removeRedundancy(evidence):
    if "_" == evidence[0]:
        evidence = ''+evidence[1:]
        return evidence
    elif "_" == evidence[-1]:
        evidence = evidence[:-1]
        return evidence
    elif "-" == evidence[0]:
        evidence = ''+evidence[1:]
        return evidence
    elif "-" == evidence[-1]:
        evidence = evidence[:-1]
        return evidence
    elif "-" in evidence:
        evidence = evidence.replace("-", "_")
        return evidence
    else:
        return evidence
