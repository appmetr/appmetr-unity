import os
from sys import argv
from mod_pbxproj import XcodeProject
import appcontroller
path = argv[1]

project = XcodeProject.Load(path +'/Unity-iPhone.xcodeproj/project.pbxproj')
project.add_file('System/Library/Frameworks/Foundation.framework', tree='SDKROOT')
project.add_file('System/Library/Frameworks/StoreKit.framework', tree='SDKROOT')

if project.modified:
    project.backup()
    project.saveFormat3_2()
	
	