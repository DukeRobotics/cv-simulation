import os
import json

# Get the current working directory
cwd = os.getcwd()

# Get the folder in current directory as input from user
folder = input("Enter the folder name: ")

# Number of files saved
kept_image_files = 0

# Number of files removed
removed_files = 0

# Loop over only subdirectories of the original folder
for subfolder in os.listdir(os.path.join(cwd, folder)):
    # Make sure subfolder is a directory
    if os.path.isdir(os.path.join(cwd, folder, subfolder)):
        # Loop over all files in the subfolder
        for file in os.listdir(os.path.join(cwd, folder, subfolder)):
            # Check if file name does not contain "step0"
            if "step0" not in file:
                # Remove the file
                os.remove(os.path.join(cwd, folder, subfolder, file))
                # Increment the number of removed files
                removed_files += 1
            elif file.lower().endswith(('.png', '.jpg', '.jpeg', '.tiff', '.bmp', '.gif')):
                kept_image_files += 1

# Print the number of removed files
print("Removed {} files".format(removed_files))

# Print the number of saved image files
print("Kept {} image files".format(kept_image_files))

# Get the folder in current directory as input from user
modify_metadata = input("Would you like to modify the folder's metadata to reflect the new number of image files? (y/n): ")

if modify_metadata == 'y' or modify_metadata == 'Y':
    # Modify the JSON metadata to reflect the new number of files
    with open(os.path.join(cwd, folder, "metadata.json"), "r+") as jsonFile:
        data = json.load(jsonFile)

        data["totalFrames"] = kept_image_files
        data["totalSequences"] = kept_image_files

        jsonFile.seek(0)
        json.dump(data, jsonFile, indent=2)
        jsonFile.truncate()

        print("Modified metadata.json to reflect the new number of image files.")
else:
    print("Did not modify metadata.json.")
