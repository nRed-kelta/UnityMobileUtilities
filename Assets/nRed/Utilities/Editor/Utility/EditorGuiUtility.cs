// nRed Unity Mobile Utilities
// Copyright (c) nRed-kelta. For terms of use, see
// https://github.com/nRed-kelta/UnityMobileUtilities

using UnityEngine;
using UnityEditor;

namespace nRed.Editor.GuiUtility
{
	//==============================================================================
	/**
	 *	@brief	GUI Utility class
	 */
	//==============================================================================
	public static class G
	{
		//==============================================================================
		/**
		*	@brief	Set GUILayout.MaxWidth.
		*
		*	@param	width	Max width value.
		*/
		//==============================================================================
		public static GUILayoutOption WIDTH(float width)
		{
			return GUILayout.MaxWidth(width);
		}

		//==============================================================================
		/**
		*	@brief	Set GUILayout.MaxHeight.
		*
		*	@param	height	Max height value.
		*/
		//==============================================================================
		public static GUILayoutOption HEIGHT(float height)
		{
			return GUILayout.MaxHeight(height);
		}
	}

	//==============================================================================
	/**
	 *	@brief	GUIStyle builder class
	 */
	//==============================================================================
	public static class GUIStyleBuilder
	{
		//==============================================================================
		/**
		*	@brief	Get or create GUIStyle.
		*
		*	@param	style	If null then create new GUIStyle. Otherwise return direct it.
		*
		*	@return	GUIStyle
		*/
		//==============================================================================
		public static GUIStyle GetOrCreate(GUIStyle style)
		{
			return style?? new GUIStyle();
		}

		//==============================================================================
		/**
		*	@brief	Set TextAnchor to GUIStyle and return it.
		*
		*	@param	style	If null then create new GUIStyle. Otherwise return direct it.
		*	@param	anchor	TextAnchor
		*
		*	@return	Builded GUIStyle
		*/
		//==============================================================================
		public static GUIStyle Align(this GUIStyle style, TextAnchor anchor=TextAnchor.MiddleLeft)
		{
			GUIStyle s = GetOrCreate(style);
			s.alignment = anchor;
			return s;
		}

		//==============================================================================
		/**
		*	@brief	Set TextClipping to GUIStyle and return it.
		*
		*	@param	style		If null then create new GUIStyle. Otherwise return direct it.
		*	@param	clipping	TextClipping
		*
		*	@return	Builded GUIStyle
		*/
		//==============================================================================
		public static GUIStyle Clipping(this GUIStyle style, TextClipping clipping=default(TextClipping))
		{
			GUIStyle s = GetOrCreate(style);
			s.clipping = clipping;
			return s;
		}

		//==============================================================================
		/**
		*	@brief	Set Vector2 contentOffset to GUIStyle and return it.
		*
		*	@param	style	If null then create new GUIStyle. Otherwise return direct it.
		*	@param	offset	contentOffset by Vector2.
		*
		*	@return	Builded GUIStyle
		*/
		//==============================================================================
		public static GUIStyle ContentOffset(this GUIStyle style, Vector2 offset=default(Vector2))
		{
			GUIStyle s = GetOrCreate(style);
			s.contentOffset = offset;
			return s;
		}

		//==============================================================================
		/**
		*	@brief	Set float x,y contentOffsets to GUIStyle and return it.
		*
		*	@param	style	If null then create new GUIStyle. Otherwise return direct it.
		*	@param	offsetx	contentOffsetx.
		*	@param	offsety	contentOffsety.
		*
		*	@return	Builded GUIStyle
		*/
		//==============================================================================
		public static GUIStyle ContentOffset(this GUIStyle style, float offsetx=0, float offsety=0)
		{
			return style.ContentOffset(new Vector2(offsetx, offsety));
		}

		//==============================================================================
		/**
		*	@brief	Set RectOffset margin to GUIStyle and return it.
		*
		*	@param	style	If null then create new GUIStyle. Otherwise return direct it.
		*	@param	margin	margin by RectOffset.
		*
		*	@return	Builded GUIStyle
		*/
		//==============================================================================
		public static GUIStyle Margin(this GUIStyle style, RectOffset margin=default(RectOffset))
		{
			GUIStyle s = GetOrCreate(style);
			s.margin = margin;
			return s;
		}

		//==============================================================================
		/**
		*	@brief	Set int left, right, top, and bottom margin to GUIStyle and return it.
		*
		*	@param	style	If null then create new GUIStyle. Otherwise return direct it.
		*	@param	left	Left margin by int.
		*	@param	right	Right margin by int.
		*	@param	top		Top margin by int.
		*	@param	bottom	Bottom margin by int.
		*
		*	@return	Builded GUIStyle
		*/
		//==============================================================================
		public static GUIStyle Margin(this GUIStyle style, int left=0, int right=0, int top=0, int bottom=0)
		{
			return style.Margin( new RectOffset(left, right, top, bottom) );
		}

		//==============================================================================
		/**
		*	@brief	Set RectOffset padding to GUIStyle and return it.
		*
		*	@param	style	If null then create new GUIStyle. Otherwise return direct it.
		*	@param	padding	padding by RectOffset.
		*
		*	@return	Builded GUIStyle
		*/
		//==============================================================================
		public static GUIStyle Padding(this GUIStyle style, RectOffset padding=default(RectOffset))
		{
			GUIStyle s = GetOrCreate(style);
			s.padding = padding;
			return s;
		}

		//==============================================================================
		/**
		*	@brief	Set int left, right, top, and bottom padding to GUIStyle and return it.
		*
		*	@param	style	If null then create new GUIStyle. Otherwise return direct it.
		*	@param	left	Left padding by int.
		*	@param	right	Right padding by int.
		*	@param	top		Top padding by int.
		*	@param	bottom	Bottom padding by int.
		*
		*	@return	Builded GUIStyle
		*/
		//==============================================================================
		public static GUIStyle Padding(this GUIStyle style, int left=0, int right=0, int top=0, int bottom=0)
		{
			return style.Padding( new RectOffset(left, right, top, bottom) );
		}

		//==============================================================================
		/**
		*	@brief	Set active text color to GUIStyle and return it.
		*
		*	@param	style	If null then create new GUIStyle. Otherwise return direct it.
		*	@param	color	Active text color by Color.
		*
		*	@return	Builded GUIStyle
		*/
		//==============================================================================
		public static GUIStyle OnActiveColor(this GUIStyle style, Color color=default(Color))
		{
			GUIStyle s = GetOrCreate(style);
			s.active.textColor = color;
			return s;
		}

		//==============================================================================
		/**
		*	@brief	Set normal text color to GUIStyle and return it.
		*
		*	@param	style	If null then create new GUIStyle. Otherwise return direct it.
		*	@param	color	Normal text color by Color.
		*
		*	@return	Builded GUIStyle
		*/
		//==============================================================================
		public static GUIStyle TextColor(this GUIStyle style, Color color=default(Color))
		{
			GUIStyle s = GetOrCreate(style);
			s.normal.textColor = color;
			return s;
		}

		//==============================================================================
		/**
		*	@brief	Set focused text color to GUIStyle and return it.
		*
		*	@param	style	If null then create new GUIStyle. Otherwise return direct it.
		*	@param	color	Focused text color by Color.
		*
		*	@return	Builded GUIStyle
		*/
		//==============================================================================
		public static GUIStyle FocusedTextColor(this GUIStyle style, Color color=default(Color))
		{
			GUIStyle s = GetOrCreate(style);
			s.focused.textColor = color;
			return s;
		}

		//==============================================================================
		/**
		*	@brief	Set hover text color to GUIStyle and return it.
		*
		*	@param	style	If null then create new GUIStyle. Otherwise return direct it.
		*	@param	color	Hover text color by Color.
		*
		*	@return	Builded GUIStyle
		*/
		//==============================================================================
		public static GUIStyle HoverTextColor(this GUIStyle style, Color color=default(Color))
		{
			GUIStyle s = GetOrCreate(style);
			s.hover.textColor = color;
			return s;
		}

		//==============================================================================
		/**
		*	@brief	Set richText to GUIStyle and return it.
		*
		*	@param	style	If null then create new GUIStyle. Otherwise return direct it.
		*	@param	onoff	on/off by bool.
		*
		*	@return	Builded GUIStyle
		*/
		//==============================================================================
		public static GUIStyle RichText(this GUIStyle style, bool onoff=true)
		{
			GUIStyle s = GetOrCreate(style);
			s.richText = onoff;
			return s;
		}

		//==============================================================================
		/**
		*	@brief	Set imagePosition to GUIStyle and return it.
		*
		*	@param	style	If null then create new GUIStyle. Otherwise return direct it.
		*	@param	imagePosition	ImagePosition.
		*
		*	@return	Builded GUIStyle
		*/
		//==============================================================================
		public static GUIStyle ImagePos(this GUIStyle style, ImagePosition imagePosition=default(ImagePosition))
		{
			GUIStyle s = GetOrCreate(style);
			s.imagePosition = imagePosition;
			return s;
		}

		//==============================================================================
		/**
		*	@brief	Set fixedWidth to GUIStyle and return it.
		*
		*	@param	style	If null then create new GUIStyle. Otherwise return direct it.
		*	@param	width	fixedWidth by float.
		*
		*	@return	Builded GUIStyle
		*/
		//==============================================================================
		public static GUIStyle FixedWidth(this GUIStyle style, float width=0)
		{
			GUIStyle s = GetOrCreate(style);
			s.fixedWidth = width;
			return s;
		}

		//==============================================================================
		/**
		*	@brief	Set fixedHeight to GUIStyle and return it.
		*
		*	@param	style	If null then create new GUIStyle. Otherwise return direct it.
		*	@param	height	fixedHeight by float.
		*
		*	@return	Builded GUIStyle
		*/
		//==============================================================================
		public static GUIStyle FixedHeight(this GUIStyle style, float height=0)
		{
			GUIStyle s = GetOrCreate(style);
			s.fixedHeight = height;
			return s;
		}

		//==============================================================================
		/**
		*	@brief	Set font to GUIStyle and return it.
		*
		*	@param	style	If null then create new GUIStyle. Otherwise return direct it.
		*	@param	font	Font.
		*
		*	@return	Builded GUIStyle
		*/
		//==============================================================================
		public static GUIStyle Font(this GUIStyle style, Font font)
		{
			GUIStyle s = GetOrCreate(style);
			s.font = font;
			return s;
		}

		//==============================================================================
		/**
		*	@brief	Set fontSize to GUIStyle and return it.
		*
		*	@param	style	If null then create new GUIStyle. Otherwise return direct it.
		*	@param	size	fontSize by int.
		*
		*	@return	Builded GUIStyle
		*/
		//==============================================================================
		public static GUIStyle DynamicFontSize(this GUIStyle style, int size)
		{
			GUIStyle s = GetOrCreate(style);
			s.fontSize = size;
			return s;
		}

		//==============================================================================
		/**
		*	@brief	Set fontStyle to GUIStyle and return it.
		*
		*	@param	style	If null then create new GUIStyle. Otherwise return direct it.
		*	@param	size	FontStyle.
		*
		*	@return	Builded GUIStyle
		*/
		//==============================================================================
		public static GUIStyle DynamicFontStyle(this GUIStyle style, FontStyle fontStyle)
		{
			GUIStyle s = GetOrCreate(style);
			s.fontStyle = fontStyle;
			return s;
		}

		//==============================================================================
		/**
		*	@brief	Set name to GUIStyle and return it.
		*
		*	@param	style	If null then create new GUIStyle. Otherwise return direct it.
		*	@param	name	Name by string.
		*
		*	@return	Builded GUIStyle
		*/
		//==============================================================================
		public static GUIStyle Name(this GUIStyle style, string name)
		{
			GUIStyle s = GetOrCreate(style);
			s.name = name;
			return s;
		}
	}

	//==============================================================================
	/**
	*	@brief	EditorGUI.Begin/EndDisabledGroup utility class
	*/
	//==============================================================================
	public class DISABLE : GUI.Scope
	{
		public DISABLE(bool condition)
		{
			EditorGUI.BeginDisabledGroup(condition);
		}


		protected override void CloseScope()
		{
			EditorGUI.EndDisabledGroup();
		}
	}

	//==============================================================================
	/**
	*	@brief	EditorGUI.Begin/EndHorizontal utility class
	*/
	//==============================================================================
	public class XLAYOUT : GUI.Scope
	{
		public XLAYOUT()
		{
			EditorGUILayout.BeginHorizontal();
		}
		public XLAYOUT(params GUILayoutOption[] options)
		{
			EditorGUILayout.BeginHorizontal(options);
		}
		public XLAYOUT(GUIStyle style, params GUILayoutOption[] options)
		{
			EditorGUILayout.BeginHorizontal(style, options);
		}


		protected override void CloseScope()
		{
			EditorGUILayout.EndHorizontal();
		}
	}

	//==============================================================================
	/**
	*	@brief	EditorGUI.Begin/EndVertical utility class
	*/
	//==============================================================================
	public class YLAYOUT : GUI.Scope
	{
		public YLAYOUT()
		{
			EditorGUILayout.BeginVertical();
		}
		public YLAYOUT(params GUILayoutOption[] options)
		{
			EditorGUILayout.BeginVertical(options);
		}
		public YLAYOUT(GUIStyle style, params GUILayoutOption[] options)
		{
			EditorGUILayout.BeginVertical(style, options);
		}


		protected override void CloseScope()
		{
			EditorGUILayout.EndVertical();
		}
	}
}