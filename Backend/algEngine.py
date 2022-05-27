from fileinput import filename

class algEngine(object):
    def __init__(self, path):
        # Setup
        fileNames = self.get_file_name(path)
        self.database = self.generate_db(path, fileNames)

    # retrieving evidence records from reference folder
    def get_file_name(self, path):
        import os

        retList = []
        path = os.path.abspath(path)

        for file in os.listdir(path): 
            retList.append(file)
        return retList

    # Building a list of dict as referencing database
    # keys are bType(blockchain type), libName(encryption library name), and chance(possibility associated with that library)
    def generate_db(self, path, fileNames):
        files = []
        for name in fileNames:
            files.append(open(path + "\\\\" + name, "rb"))

        db = []
        for f in files:
            content = f.read()
            row = self.extract_db_content(content)
            db = db + row

        return db
    
    # Extracting contents of files
    def extract_db_content(self, content):
        results = content.split( )

        # ignore header
        results = results[6:]
        tempRow = []
        for result in results:
            result = result.decode().split(":")
            if(len(result) == 3):
                tempRow.append({"bType":result[0], "libName":result[1], "chance":result[2]})

        return tempRow

    # recieving data, processing data and, returning data
    def process_upload(self, data):
        libraries = data
        chances = self.check_for_chance(libraries)
        retBody = self.prepare_ret(chances)

        return retBody

    # exclude the libraries that are not in our reference database
    def exclude_unrelated(self, libraries):
        retList = []

        for library in libraries:
            for db in self.database:
                if library == db["libName"]:
                    retList.append(library)

        return retList

    # Similarity calculation - calculates chances by matching related libraries with referencing database.
    def check_for_chance(self, libraries):
        result = []
        length = len(libraries)

        for lib in libraries:
            for entry in self.database:
                if lib == entry["libName"]:
                    bExisted = False
                    chance = float(entry["chance"])

                    for r in result:
                        if(r["bType"] == entry["bType"]):
                            bExisted = True
                            r["chance"] = r["chance"] + chance / length

                            break

                    if not bExisted:
                        result.append({"bType": entry["bType"], "chance": chance / length})

        return self.normalize_chance(result)

    # Make the final score out of 100
    def normalize_chance(self, chances):
        if len(chances) > 3:
            chances = chances[0:3]

        total = 0
        for i in range(len(chances)):
            chances[i]["chance"] = round(chances[i]["chance"] * 100, 1)
            total += chances[i]["chance"]

        tempT = 0
        for i in range(len(chances)):
            if i != len(chances) - 1:
                temp = round(chances[i]["chance"] / total * 100)
                tempT += temp
            else:
                temp = 100 - tempT

            chances[i]["chance"] = temp

        return chances

    # Build return body for response
    def prepare_ret(self, chances):
        chances = sorted(chances, key=lambda e: e.__getitem__('chance'), reverse= True)
        if(len(chances) == 0):
            retBody = {"code": 404, "msg": "Sorry, no match found."}
        else:
            retBody = {"code": 333, "data": chances}


        return retBody