Welcome to this Asset I'll show you the server with MySQL and the client to it.

Installation:

- Create new project whith random name, cut the "Client" of "ServerAndClient" and insert in "asset" on your new project.

- You want to install MySQL. If you do not have extensive knowledge of installing MySQL, I recommend to take this free version from the official site "http://dev.mysql.com/downloads/installer".

- after installing MYSQL you needed run "Server".
1. Click on button "1" enter your MYSQL "IP, port, userID, password" and click "Connect".
2. Click on button "2". If your need create new DB, enter random(A-Z, a-z, 0-9) name and click "Create DB", click "AddTable". If you have DB, select find DB in "Chose DB" and click "AddTable" else if you have DB and tables in DB, select find DB in "ChoseDB".
3. In "Server run" click button 1 "MySQL Connect", 
2 "Server Start".

- Launch your new client project and start "LoginScreen".  Registration new account. Sign to game and create character.

Info: Build game first scene "LoginScene", second "Game".

Options:
- Place "true" in Edit->Project Setting->Player->Resolution->
Run in background

- If on build you have this error "Error building Player: Extracting referenced dlls failed.". Please click "Edit - ProjectSetting - Player" and in "Settings for PC, MAC & Linux" select ".Net 2.0" in row "API Compatibility Level".

- For send password on e-mail you need to adjust script in Server->MySQL->SQL_PasswordRecovery.cs

- If you want to allow multiple users join from one account, check "Test Mode = true". But with this installation of the server may work not correctly.

- Configure the port on which the server will run you on stage in gameobject "Networking". Configure port and IP on the client, you can on scene "LoginScene" in gameobject "Networking".

Game:
- You can send private message write symbol "[nick] [text message].

Mobile platform:
- Models in this asset don't optimized

BIG THANKS! :
- Mixamo.com for the free animation and characters.
- Black Curtain Studio for Free Scifi Gun Collection. 
Link: https://www.assetstore.unity3d.com/en/#!/content/56350

Wait in next update:
- Health bar
- Rang system
- Weapon change system

If you have any problems or you find an error, please notify me by email "elestr@bk.ru".