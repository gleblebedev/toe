using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using tom;

namespace Toe.ToeVsExt
{
	// Last command type sent to the macro recorder. Note that there are more commands
	// recorded than is implied by this list. Commands in this list (other than
	// LastMacroNone) are coalesced when multiples of the same command are received
	// consecutively.

	// This enum should be extended or replaced with your own command identifiers to enable
	// Coalescing of commands.
	public enum LastMacro
	{
		None,

		Text,

		DownArrowLine,

		DownArrowLineSelection,

		DownArrowPara,

		DownArrowParaSelection,

		UpArrowLine,

		UpArrowLineSelection,

		UpArrowPara,

		UpArrowParaSelection,

		LeftArrowChar,

		LeftArrowCharSelection,

		LeftArrowWord,

		LeftArrowWordSelection,

		RightArrowChar,

		RightArrowCharSelection,

		RightArrowWord,

		RightArrowWordSelection,

		DeleteChar,

		DeleteWord,

		BackspaceChar,

		BackspaceWord
	}

	[SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags")]
	[SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
	public enum MoveScope
	{
		Character = tomConstants.tomCharacter,

		Word = tomConstants.tomWord,

		Line = tomConstants.tomLine,

		Paragraph = tomConstants.tomParagraph
	}

	/// <summary>
	/// The VSMacroRecorder class implementation and the IVsMacroRecorder Interface definition
	/// were included here in this seperate class because they were not included in the 
	/// interop assemblies shipped with Visual Studio 2005.
	/// 
	/// When implementing a macro recorder this class should be copied into your own name space
	/// and not shared between different 3rd party packages.
	/// </summary>
	public class VSMacroRecorder
	{
		#region Constants and Fields

		private Guid m_GuidEmitter;

		private LastMacro m_LastMacroRecorded;

		private uint m_TimesPreviouslyRecorded;

		private IVsMacroRecorder m_VsMacroRecorder;

		#endregion

		#region Constructors and Destructors

		public VSMacroRecorder(Guid emitter)
		{
			this.m_LastMacroRecorded = LastMacro.None;

			this.m_GuidEmitter = emitter;
		}

		#endregion

		#region Public Methods and Operators

		public uint GetTimesPreviouslyRecorded(LastMacro macro)
		{
			return this.IsLastRecordedMacro(macro) ? this.m_TimesPreviouslyRecorded : 0;
		}

		// Compiler generated destructor is fine

		public bool IsLastRecordedMacro(LastMacro macro)
		{
			return (macro == this.m_LastMacroRecorded && this.ObjectIsLastMacroEmitter()) ? true : false;
		}

		public bool IsRecording()
		{
			// If the property can not be retreived it is assumeed no macro is being recorded.
			VSRECORDSTATE recordState = VSRECORDSTATE.VSRECORDSTATE_OFF;

			// Retrieve the macro recording state.
			IVsShell vsShell = (IVsShell)Package.GetGlobalService(typeof(SVsShell));
			if (vsShell != null)
			{
				object var;
				if (ErrorHandler.Succeeded(vsShell.GetProperty((int)__VSSPROPID.VSSPROPID_RecordState, out var)) && null != var)
				{
					recordState = (VSRECORDSTATE)var;
				}
			}

			// If there is a change in the record state to OFF or ON we must either obtain
			// or release the macro recorder. 
			if (recordState == VSRECORDSTATE.VSRECORDSTATE_ON && this.m_VsMacroRecorder == null)
			{
				// If this QueryService fails we no macro recording
				this.m_VsMacroRecorder = (IVsMacroRecorder)Package.GetGlobalService(typeof(IVsMacroRecorder));
			}
			else if (recordState == VSRECORDSTATE.VSRECORDSTATE_OFF && this.m_VsMacroRecorder != null)
			{
				// If the macro recording state has been switched off then we can release
				// the service. Note that if the state has become paused we take no action.
				this.Stop();
			}

			return (this.m_VsMacroRecorder != null);
		}

		public bool RecordBatchedLine(LastMacro macroRecorded, string line)
		{
			if (null == line)
			{
				line = "";
			}

			return this.RecordBatchedLine(macroRecorded, line, 0);
		}

		public bool RecordBatchedLine(LastMacro macroRecorded, string line, int maxLineLength)
		{
			if (null == line)
			{
				line = "";
			}

			if (maxLineLength > 0 && line.Length >= maxLineLength)
			{
				// Reset the state after recording the line, so it will not be appended to further
				this.RecordLine(line);
				// Notify the caller that the this line will not be appended to further
				return true;
			}

			if (this.IsLastRecordedMacro(macroRecorded))
			{
				this.m_VsMacroRecorder.ReplaceLine(line, ref this.m_GuidEmitter);
				// m_LastMacroRecorded can stay the same
				++this.m_TimesPreviouslyRecorded;
			}
			else
			{
				this.m_VsMacroRecorder.RecordLine(line, ref this.m_GuidEmitter);
				this.m_LastMacroRecorded = macroRecorded;
				this.m_TimesPreviouslyRecorded = 1;
			}

			return false;
		}

		public void RecordLine(string line)
		{
			this.m_VsMacroRecorder.RecordLine(line, ref this.m_GuidEmitter);
			this.Reset();
		}

		public void Reset()
		{
			this.m_LastMacroRecorded = LastMacro.None;
			this.m_TimesPreviouslyRecorded = 0;
		}

		public void Stop()
		{
			this.Reset();
			this.m_VsMacroRecorder = null;
		}

		#endregion

		// This function determines if the last line sent to the macro recorder was
		// sent from this emitter. Note it is not valid to call this function if
		// macro recording is switched off.

		#region Methods

		private bool ObjectIsLastMacroEmitter()
		{
			Guid guid;
			this.m_VsMacroRecorder.GetLastEmitterId(out guid);
			return guid.Equals(this.m_GuidEmitter);
		}

		#endregion
	}

	#region "IVsMacro Interfaces"

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	[ComConversionLoss]
	internal struct _VSPROPSHEETPAGE
	{
		public uint dwSize;

		public uint dwFlags;

		[ComAliasName("vbapkg.ULONG_PTR")]
		public uint hInstance;

		public ushort wTemplateId;

		public uint dwTemplateSize;

		[ComConversionLoss]
		public IntPtr pTemplate;

		[ComAliasName("vbapkg.ULONG_PTR")]
		public uint pfnDlgProc;

		[ComAliasName("vbapkg.LONG_PTR")]
		public int lParam;

		[ComAliasName("vbapkg.ULONG_PTR")]
		public uint pfnCallback;

		[ComConversionLoss]
		public IntPtr pcRefParent;

		public uint dwReserved;

		[ComConversionLoss]
		[ComAliasName("vbapkg.wireHWND")]
		public IntPtr hwndDlg;
	}

	internal enum _VSRECORDMODE
	{
		// Fields
		VSRECORDMODE_ABSOLUTE = 1,

		VSRECORDMODE_RELATIVE = 2
	}

	[ComImport]
	[ComConversionLoss]
	[InterfaceType(1)]
	[Guid("55ED27C1-4CE7-11D2-890F-0060083196C6")]
	internal interface IVsMacros
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetMacroCommands([Out] IntPtr ppsaMacroCanonicalNames);
	}

	[ComImport]
	[InterfaceType(1)]
	[Guid("04BBF6A5-4697-11D2-890E-0060083196C6")]
	internal interface IVsMacroRecorder
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void RecordStart([In] [MarshalAs(UnmanagedType.LPWStr)] string pszReserved);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void RecordEnd();

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void RecordLine([In] [MarshalAs(UnmanagedType.LPWStr)] string pszLine, [In] ref Guid rguidEmitter);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetLastEmitterId([Out] out Guid pguidEmitter);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void ReplaceLine([In] [MarshalAs(UnmanagedType.LPWStr)] string pszLine, [In] ref Guid rguidEmitter);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void RecordCancel();

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void RecordPause();

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void RecordResume();

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetCodeEmittedFlag([In] int fFlag);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetCodeEmittedFlag([Out] out int pfFlag);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetKeyWord([In] uint uiKeyWordId, [Out] [MarshalAs(UnmanagedType.BStr)] out string pbstrKeyWord);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void IsValidIdentifier([In] [MarshalAs(UnmanagedType.LPWStr)] string pszIdentifier);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetRecordMode([Out] out _VSRECORDMODE peRecordMode);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetRecordMode([In] _VSRECORDMODE eRecordMode);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetStringLiteralExpression(
			[In] [MarshalAs(UnmanagedType.LPWStr)] string pszStringValue,
			[Out] [MarshalAs(UnmanagedType.BStr)] out string pbstrLiteralExpression);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void ExecuteLine([In] [MarshalAs(UnmanagedType.LPWStr)] string pszLine);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void AddTypeLibRef([In] ref Guid guidTypeLib, [In] uint uVerMaj, [In] uint uVerMin);
	}

	#endregion
}