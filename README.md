#XNInterface

*Important Note: This readme pertains to the Develop branch of this repository only. Please use that branch for the latest version of the library.*

XNInterface is a GUI library for XNA 4.0, and is compatible and tested with Windows and WP7. (It may be compatible with the XBOX however this has not been tested - let me know if it works for you!)

##Usage
First you will want to build the source, this should be easy enough assuming you have XNA Game Studio 4.0 installed.

Once you have gotten the DLLs out of the build folder, add them to your project. XNInterface.dll goes with your game, and XNInterfaceCompilerLib.dll as well as XNInterfaceImporters.dll go into your Content project.

Now you can create a .xml file and start designing your GUI. Here is one to get you started:

	<?xml version="1.0" encoding="utf-8" ?>
	<Window Size="100%">
	  <Button Name="btnExit" Text="Exit">
	    <TextBlock Font="Fonts\EditorFont.spritefont" Color="#FFFFFF" Format="{parenttext}" />
	  </Button>
	</Window>

This will create a Button with the text Exit in white and place it at the top left corner.

Now inside your game code you need to do the following:

1 - First declare a Window object and create a SpriteBatch.

2 - Next load the Window using the ContentManager, initialise and Window, and then Load the content for the window:

	_mainWindow = _content.Load<Window>("GUI\\Editor"); 
	_mainWindow.Initialise(null); 
	_mainWindow.LoadGraphics(Manager.Game.GraphicsDevice, _content); 

3 - Inside your update function, add the following:

	_mainWindow.PerformLayout(Manager.Game.GraphicsDevice.Viewport.Width, Manager.Game.GraphicsDevice.Viewport.Height);
	_mainWindow.Update(time.ElapsedGameTime.TotalSeconds);

4 - Finally inside your Draw method add the following:

	_sb.Begin();
	_mainWindow.Draw(Manager.Game.GraphicsDevice, _sb, time.ElapsedGameTime.TotalSeconds);
	_sb.End();

Now you should have a visible Exit button. To register with the Triggered event on the button, simply use the GetChild generic method inside your root control (the Window) to find the button by name, and register the event. Note however that this will not work as-is, and requires an Input Controller to handle the input and trigger the event.

##Input Controllers
XNInterface uses a separate class to handle input, which is then linked to the root control of your GUI to enable interaction.

The following will describe adding Mouse support to the window you created above.

1 - Create a MouseInput object, and pass the Window you created earlier to the constructor.

2 - Add the following before your Window update code inside the Update function:

	_mainWindowMouse.HandleMouse(Mouse.GetState());

3 - Try it out!

Now you should be able to click on the exit button, and whatever code you have bound to the Triggered event should now execute.

## Custom Controls
To create a custom control, you need to do two things:
1 - Create the Control in a DLL separate to your Game Executable (Important)
2 - Create a xninterface_assemblylist.txt file in your Content folder and enter the path to the DLL

A control is a class that inherits from BaseControl, and also has a special attribute which describes the name and max # of children the control can hold.
	
	[XNIControl("XMLNAME", NumChildren)]

Once you have everything setup as described, you should be able to just refer to the control in your XML as normal, and everything will work properly.

##License
###Microsoft Public License (MS-PL)
This license governs use of the accompanying software. If you use the software, you
accept this license. If you do not accept the license, do not use the software.

1. Definitions
The terms "reproduce," "reproduction," "derivative works," and "distribution" have the same meaning here as under U.S. copyright law.  
A "contribution" is the original software, or any additions or changes to the software.  
A "contributor" is any person that distributes its contribution under this license.  
"Licensed patents" are a contributor's patent claims that read directly on its contribution.

2. Grant of Rights  
(A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.  
(B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.

3. Conditions and Limitations  
(A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.  
(B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from such contributor to the software ends automatically.  
(C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present in the software.  
(D) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a license that complies with this license.  
(E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement.