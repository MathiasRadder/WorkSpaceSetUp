# WorkSpaceSetUp
A Windows-form project that sets up your desired work space, for work or for relaxing. So that you don’t have to open each file or folder one by one.

## why?
This project was created to learn .NET, Windows-form and unit testing. 

## What?
In a plain manner this project uses a container and gives you the option to save paths, name those groups of paths and save it on a JSON file. When clicking on the group name, all the path files and folders open.

## MVVM structure
On this project I did my best to apply the MVVM structure, this also helped in unit testing. The main focus was the local data separation. 

<img src="https://github.com/user-attachments/assets/76d9d642-547b-47b9-bb89-3bf79e9d032e" width="500">


## Unit testing
My first time unit testing was a bit complex due to the project using Windows-form, all the testing was done through the view-model 
InteractionHandler class). So that I can test as accurately as I can. This way I also avoided interacting with the error handler, so that no message boxes would pop up.

<img src="https://github.com/user-attachments/assets/01f04413-6193-492d-8fa4-38630e9a83b2" width="500">

## Error handling
Error handling was unique in this project and I did not want it to respond incorrectly to unit testing. So I made it separate in the view-model, it looks at the returned result from the InteractionHandler and handles it when an error has occurred. So that it can give the user information on what happened and gives them the ability to act upon it.

<img src="https://github.com/user-attachments/assets/f3232ebe-75eb-40d4-ae66-2cb836a4a271" width="500">


## Extra info
In the FileGroup and FileGroupManager class, I added a unique way of using a container and sorting it. This was to learn new techniques and use more algorithms. 
