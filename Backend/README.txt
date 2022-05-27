Dependencies：
	See in requirements.txt.
	You can use the following command to install all dependencies at once:
		pip install -r requirements.txt

File introduction:
	server.py: 
	Responsible for deploying web server.

	framework.py: 
	Responsible for handling requests initiated from 
	frontend and preparing the response. It utilizes the Flask 
	framework where only necessary features are imported into 
	the project, including Flask.Session, Request, etc. 
	Also, modules like Shutil, OS, and part of werkzeug.utils 
	are used to handle file access. A uniformed session ID 
	received from the front end will be used through the 
	process and makes tracking user input possible by 
	structuring a global session ID system. Also, PDFKit has 
	been introduced to generate PDF reports of user’s 
	analyzing results.

	algEngine.py: 
	This where our similarity algorithm is coded 
	and assists with the calculation. It checks our database 
	of features of different block-chain systems and compares 
	the result from Crypto Detector of user-uploaded files with 
	the database. Then a similarity algorithm is used to find 
	out the most likely result. At last, a normalizing 
	algorithm helps to scale result scores and makes the 
	result more accurate.

	./utility: 
	This folder contains helper methods such as to handle 
	and parse requests from the front-end or calling Crypto 
	Detector. Also, it creates and deletes many necessary 
	temporary or permanent files along the process.

	./cryptoes ,  ./txt, ./storage:
	These three folders are reuqired for this project to run
	without error.


How to run:
	1. Make sure you installed all needed dependent libraries.

	2. Open a console and in the root directory("backend"), type this:
		 python server.py

	then, the program should be running on http://localhost:8001/ 

	You can try access this address in ur browser.
	Once you see "Deployed", it means the backend is up and runnning.
