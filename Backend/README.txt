Python Version：Python 3.6.8 (tags/v3.6.8:3c6b436a57, Dec 24 2018, 00:16:47) [MSC v.1916 64 bit (AMD64)] on win32

Dependencies：

	# framework.py:
		from flask import Flask, render_template, jsonify, session, request
		from flask_session import Session
		from werkzeug.utils import secure_filename

		import shutil

	This script using Flask, Flask_Session, and Werkzeug to handle web requests.
	Also, Shutil are used to handle file OS.

	# algEngine.py
	## no new dependency

	The actual computation happens in the following function:
		def check_for_chance(self, libraries)

How to run:
	1. Make sure you installed all needed dependent libraries.

	2. Open a console and in the root directory("CDBackEnd"), type this:
		 python server.py

	then, the program should be running on http://localhost:8001/ 

	You can try access this address in ur browser.
	Once you see "Deployed", it means the backend is up and runnning.

	Use "POST" for http://localhost:8001/file_upload.
	The required parameters are:
		1. sessionID: a unique ID for processing.
		2. userUpload: where the github repo address, zip, txt, or single source code file resides.


