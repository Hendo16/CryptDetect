#!/usr/bin/python3

"""
Copyright (c) 2017 Wind River Systems, Inc.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at:

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software  distributed
under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES
OR CONDITIONS OF ANY KIND, either express or implied.


Encryption Identification Scanner command line interface
"""

import sys

if sys.version_info[0] < 3 or (sys.version_info[0] == 3 and sys.version_info[1] < 4):
    print("Unsupported Python version " + str(sys.version))
    print("\nRequires Python version 3.4 or later.")
    sys.exit(1)

import traceback
from cryptodetector import CryptoDetector, Output, Options, Logger, FileLister
from cryptodetector.exceptions import CryptoDetectorError

sessionID = sys.argv[len(sys.argv) - 1]
sys.argv = sys.argv[:len(sys.argv) - 1]

if __name__ == '__main__':
    try:
        print('Scaning.')
        log_output_directory = None
        options = Options(CryptoDetector.VERSION).read_all_options()
        #if "log" in options:
            #if options["log"]:
        #log_output_directory = options["output"]
        CryptoDetector(options).scan(sessionID)

        print('Successfully finished scaning process.\n')

    except CryptoDetectorError as expn:
        Output.print_error(str(expn))
        if log_output_directory: Logger.write_log_files(log_output_directory)
        FileLister.cleanup_all_tmp_files()

    except KeyboardInterrupt:
        FileLister.cleanup_all_tmp_files()
        raise

    except Exception as expn:
        Output.print_error("Unhandled exception.\n\n" + str(traceback.format_exc()))
        if log_output_directory: Logger.write_log_files(log_output_directory)
        FileLister.cleanup_all_tmp_files()



#use subprocess to run 
    #python3 scan-for-crypto.py https://github.com/etotheipi/BitcoinArmory.git SessionID

    #they are creating a SessionID.crypto file  

#algengine.py(sessionID)


#python3 scan-for-crypto.py https://github.com/etotheipi/BitcoinArmory.git sessionID