﻿using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;

using tom;

namespace Toe.ToeVsExt
{
	public partial class MyEditor : UserControl
	{
		#region Constants and Fields

		private const int GetOleInterfaceCommandId = 1084;

		private readonly VSMacroRecorder m_Recorder;

		private string m_TextToRecord;

		#endregion

		#region Constructors and Destructors

		public MyEditor()
		{
			this.InitializeComponent();
			//this.richTextBoxCtrl.WordWrap = false;
			//this.richTextBoxCtrl.HideSelection = false;

			this.m_Recorder = new VSMacroRecorder(GuidList.guidToeVisualStudioExtensionEditorFactory);
		}

		#endregion

		//public EditorTextBox RichTextBoxControl
		//{
		//    get { return this.richTextBoxCtrl; }
		//}

		#region Public Properties

		public bool Overstrike { get; set; }

		#endregion

		#region Public Methods and Operators

		public void LoadFile(string fileName)
		{
		}

		public void RecordCommand(string command)
		{
			if (this.m_Recorder.IsRecording())
			{
				string line = "ActiveDocument.Object.";

				line += command;

				this.m_Recorder.RecordLine(line);
			}
		}

		//private ITextDocument textDocument;

		///// <summary>
		///// This property exposes the ITextDocument interface associated with
		///// our Rich Text editor.
		///// </summary>
		//public ITextDocument TextDocument
		//{
		//    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		//    get
		//    {
		//        if (null != textDocument)
		//            return textDocument;

		//        // To get the IRichEditOle interface we need to call SendMessage, which
		//        // we imported from user32.dll
		//        object editOle = null;
		//        NativeMethods.SendMessage(richTextBoxCtrl.Handle,                // The rich text box handle
		//                                  GetOleInterfaceCommandId,   // The command ID for EM_GETOLEINTERFACE
		//                                  IntPtr.Zero,                // null
		//                                  out editOle                 // This will be set to the IRichEditOle interface
		//                                  );

		//        // Call GetIUnknownForObject with the IRichEditOle interface that we
		//        // just got so that we have an IntPtr to pass into QueryInterface
		//        IntPtr editOlePtr = IntPtr.Zero;
		//        editOlePtr = Marshal.GetIUnknownForObject(editOle);

		//        // Call QueryInterface to get the pointer to the ITextDocument
		//        IntPtr iTextDocument = IntPtr.Zero;
		//        Guid iTextDocumentGuid = typeof(ITextDocument).GUID;
		//        Marshal.QueryInterface(editOlePtr, ref iTextDocumentGuid, out iTextDocument);

		//        // We need to call Marshal.Release with the pointer that we got
		//        // from the GetIUnknownForObject call
		//        Marshal.Release(editOlePtr);

		//        // Call GetObjectForIUnknown passing in the pointer that was set
		//        // by QueryInterface and return it as an ITextDocument
		//        textDocument = Marshal.GetObjectForIUnknown(iTextDocument) as ITextDocument;
		//        return textDocument;
		//    }
		//}

		///// <summary>
		///// This property will return the current ITextRange interface.
		///// </summary>
		//public ITextRange TextRange
		//{
		//    get
		//    {
		//        return this.TextDocument.Range(0, (int)tom.tomConstants.tomForward);
		//    }
		//}

		///// <summary>
		///// This property will return the current ITextSelection interface.
		///// </summary>
		//public ITextSelection TextSelection
		//{
		//    get
		//    {
		//        return this.TextDocument.Selection;
		//    }
		//}

		///// <summary>
		///// Returns the column number from the specified index
		///// </summary>
		///// <param name="index">index of the character</param>
		///// <returns>column number</returns>
		//public int GetColumnFromIndex(int index)
		//{
		//    //first get the index of the first char of the current line
		//    int currentLineIndex = richTextBoxCtrl.GetFirstCharIndexOfCurrentLine();
		//    return index - currentLineIndex;
		//}

		///// <summary>
		///// Returns the index from the specified line and column number
		///// </summary>
		///// <param name="line">line number</param>
		///// <param name="column">column number</param>
		///// <returns>index</returns>
		//public int GetIndexFromLineAndColumn(int line, int column)
		//{
		//    if (line < 0)
		//        return -1;
		//    //first get the index of the first char of the specified line
		//    int firstCharLineIndex = richTextBoxCtrl.GetFirstCharIndexFromLine(line);
		//    if (firstCharLineIndex < 0)
		//        return -1;

		//    return firstCharLineIndex + column;
		//}

		public void RecordDelete(bool backspace, bool word)
		{
			// If not backspace then it's a delete
			// If not word then it's a single character
			LastMacro macroType = backspace
			                      	? (word ? LastMacro.BackspaceWord : LastMacro.BackspaceChar)
			                      	: (word ? LastMacro.DeleteWord : LastMacro.DeleteChar);

			// Get the number of times the macro type calculated above has been recorded already
			// (if any) and then add one to get the current count
			uint count = this.m_Recorder.GetTimesPreviouslyRecorded(macroType) + 1;

			string macroString = "";
			// if this parameter is negative, it indicates a backspace, rather then a delete
			macroString += "ActiveDocument.Object.Delete(" + (int)(word ? tomConstants.tomWord : tomConstants.tomCharacter)
			               + ", " + (backspace ? -1 * count : count) + ")";

			this.m_Recorder.RecordBatchedLine(macroType, macroString);
		}

		public void RecordMove(LastMacro state, string direction, MoveScope scope, bool extend)
		{
			string macroString = "";
			macroString += "ActiveDocument.Object.Move";
			macroString += direction;
			// Get the number of times this macro type has been recorded already
			// (if any) and then add one to get the current count
			macroString += "(" + (int)scope + ", " + (this.m_Recorder.GetTimesPreviouslyRecorded(state) + 1) + ", "
			               + (int)(extend ? tomConstants.tomExtend : tomConstants.tomMove) + ")";

			this.m_Recorder.RecordBatchedLine(state, macroString);
		}

		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
		public void RecordNonprintableChar(Keys currentKey)
		{
			string macroString = "";

			// Obtain the CTRL and SHIFT as they modify a number of the virtual keys. 
			bool shiftDown = Keys.Shift == (ModifierKeys & Keys.Shift); //Keyboard::IsKeyDown(VK_SHIFT);
			bool controlDown = Keys.Control == (ModifierKeys & Keys.Control); //Keyboard::IsKeyDown(VK_CONTROL);

			// msg.WParam indicates the virtual key.
			switch (currentKey)
			{
				case Keys.Back: // BackSpace key
					// Note that SHIFT does not affect this command
					this.RecordDelete(true, controlDown);
					break;

				case Keys.Delete:
					// Note that SHIFT completely disables this command
					if (!shiftDown)
					{
						this.RecordDelete(false, controlDown);
					}
					break;

				case Keys.Left: // Left Arrow
					// SHIFT indicates selection, CTRL indicates words instead of characters
					{
						LastMacro macroType = controlDown
						                      	? (shiftDown ? LastMacro.LeftArrowWordSelection : LastMacro.LeftArrowWord)
						                      	: (shiftDown ? LastMacro.LeftArrowCharSelection : LastMacro.LeftArrowChar);

						this.RecordMove(macroType, "Left", controlDown ? MoveScope.Word : MoveScope.Character, shiftDown);
					}
					break;

				case Keys.Right: // Right Arrow
					// SHIFT indicates selection, CTRL indicates words instead of characters
					{
						LastMacro macroType = controlDown
						                      	? (shiftDown ? LastMacro.RightArrowWordSelection : LastMacro.RightArrowWord)
						                      	: (shiftDown ? LastMacro.RightArrowCharSelection : LastMacro.RightArrowChar);

						this.RecordMove(macroType, "Right", controlDown ? MoveScope.Word : MoveScope.Character, shiftDown);
					}
					break;

				case Keys.Up: // Up Arrow
					// SHIFT indicates selection, CTRL indicates paragraphs instead of lines
					{
						LastMacro macroType = controlDown
						                      	? (shiftDown ? LastMacro.UpArrowParaSelection : LastMacro.UpArrowPara)
						                      	: (shiftDown ? LastMacro.UpArrowLineSelection : LastMacro.UpArrowLine);

						this.RecordMove(macroType, "Up", controlDown ? MoveScope.Paragraph : MoveScope.Line, shiftDown);
					}
					break;

				case Keys.Down: // Down Arrow
					// SHIFT indicates selection, CTRL indicates paragraphs instead of lines
					{
						LastMacro macroType = controlDown
						                      	? (shiftDown ? LastMacro.DownArrowParaSelection : LastMacro.DownArrowPara)
						                      	: (shiftDown ? LastMacro.DownArrowLineSelection : LastMacro.DownArrowLine);

						this.RecordMove(macroType, "Down", controlDown ? MoveScope.Paragraph : MoveScope.Line, shiftDown);
					}
					break;

				case Keys.Prior: // Page Up
				case Keys.Next: // Page Down
					macroString += "ActiveDocument.Object.Move";

					if (Keys.Prior == currentKey)
					{
						macroString += "Up";
					}
					else
					{
						macroString += "Down";
					}

					macroString += "(" + (int)(controlDown ? tomConstants.tomWindow : tomConstants.tomScreen) + ", 1, "
					               + (int)(shiftDown ? tomConstants.tomExtend : tomConstants.tomMove) + ")";

					this.m_Recorder.RecordLine(macroString);
					break;

				case Keys.End:
				case Keys.Home:
					macroString += "ActiveDocument.Object.";

					if (Keys.End == currentKey)
					{
						macroString += "EndKey";
					}
					else
					{
						macroString += "HomeKey";
					}

					macroString += "(" + (int)(controlDown ? tomConstants.tomStory : tomConstants.tomLine) + ", "
					               + (int)(shiftDown ? tomConstants.tomExtend : tomConstants.tomMove) + ")";

					this.m_Recorder.RecordLine(macroString);
					break;

				case Keys.Insert:
					// Note that the CTRL completely disables this command.  Also the SHIFT+INSERT
					// actually generates a WM_PASTE message rather than a WM_KEYDOWN
					if (!controlDown)
					{
						macroString = "ActiveDocument.Object.Flags = ActiveDocument.Object.Flags Xor ";
						macroString += (int)tomConstants.tomSelOvertype;
						this.m_Recorder.RecordLine(macroString);
					}
					break;
			}
		}

		public void RecordPrintableChar(char currentValue)
		{
			string macroString = "";

			if (!this.m_Recorder.IsLastRecordedMacro(LastMacro.Text))
			{
				this.m_TextToRecord = "";
			}

			// Only deal with text characters.  Everything, space and above is a text chracter
			// except DEL (0x7f).  Include carriage return (enter key) and tab, which are
			// below space, since those are also text characters.
			if (char.IsLetterOrDigit(currentValue) || char.IsPunctuation(currentValue) || char.IsSeparator(currentValue)
			    || char.IsSymbol(currentValue) || char.IsWhiteSpace(currentValue) || '\r' == currentValue || '\t' == currentValue)
			{
				if ('\r' == currentValue)
				{
					// Emit "\r\n" as the standard line terminator
					this.m_TextToRecord += "\" & vbCr & \"";
				}
				else if ('\t' == currentValue)
				{
					// Emit "\t" as the standard tab
					this.m_TextToRecord += "\" & vbTab & \"";
				}
				else
				{
					this.m_TextToRecord += currentValue;
				}

				macroString += "ActiveDocument.Object.TypeText(\"";
				macroString += this.m_TextToRecord;
				macroString += "\")";

				if (this.m_Recorder.RecordBatchedLine(LastMacro.Text, macroString, 100)) // arbitrary max length
				{
					// Clear out the buffer if the line hit max length, since
					// it will not continue to be appended to
					this.m_TextToRecord = "";
				}
			}
		}

		public void SaveFile(string fileName)
		{
		}

		public void StopRecorder()
		{
			this.m_Recorder.Stop();
		}

		#endregion

		// This event returns the literal key that was pressed and does not account for
		// case of characters.  KeyPress is used to handled printable caracters.

		#region Methods

		private void richTextBoxCtrl_KeyDown(object sender, KeyEventArgs e)
		{
			if (this.m_Recorder.IsRecording())
			{
				this.RecordNonprintableChar(e.KeyCode);
			}
		}

		// The argumements of this event will give us the char value of the key press taking into
		// account other characters press such as shift or caps lock for proper casing.
		private void richTextBoxCtrl_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (this.m_Recorder.IsRecording())
			{
				this.RecordPrintableChar(e.KeyChar);
			}
		}

		#endregion

		//private void richTextBoxCtrl_MouseEnter(object sender, EventArgs e)
		//{
		//    if(m_Recorder.IsRecording())
		//        richTextBoxCtrl.FilterMouseClickMessages = true;
		//    else
		//        richTextBoxCtrl.FilterMouseClickMessages = false;
		//}
	}
}