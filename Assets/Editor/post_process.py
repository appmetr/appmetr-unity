import os
import sys, os.path
from sys import argv
from mod_pbxproj import XcodeProject

path = argv[1]

project = XcodeProject.Load(path +'/Unity-iPhone.xcodeproj/project.pbxproj')
project.add_file('System/Library/Frameworks/Foundation.framework', tree='SDKROOT')
project.add_file('System/Library/Frameworks/StoreKit.framework', tree='SDKROOT')

if project.modified:
    project.backup()
    project.saveFormat3_2()

	
install_path = sys.argv[1]
target_platform = sys.argv[2]
 
if target_platform != "iPhone": sys.exit()
 
info_plist_path = os.path.join(install_path, 'Info.plist')
 
file = open(info_plist_path, 'r')
plist = file.read()
file.close()
 
elements_to_add = '''
<key>appmetrUrl</key>
<string>http://stage.pixapi.net/pixapi</string>
'''
 
plist = plist.replace('<key>', elements_to_add + '<key>', 1)
 
file = open(info_plist_path, 'w')
file.write(plist)
file.close()
	