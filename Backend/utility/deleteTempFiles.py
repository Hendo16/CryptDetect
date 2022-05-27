import os
import shutil

# Delete temp storage files inside ./storage
# and temp .crypto file inside ./cryptoes
def deleteTempFiles(sessionID):
    print("Clearing temp folders and files:")
    
    if os.path.exists('storage\\' + sessionID):
        shutil.rmtree('storage\\' + sessionID)
        print("\tTemp storage dir deleted.")

    if os.path.exists('cryptoes\\' + sessionID + ".crypto"):
        os.remove('cryptoes\\' + sessionID + ".crypto")
        print("\tTemp crypto file deleted.")
    
    print("Clearing process finished.")

