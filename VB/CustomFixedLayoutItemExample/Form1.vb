Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraLayout

Namespace CustomFixedLayoutItemExample
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
'			#Region "#1"
			layoutControl1.RegisterCustomPropertyGridWrapper(GetType(MyFixedLabelItem), GetType(MyFixedLabelPropertiesWrapper))
			layoutControl1.RegisterFixedItemType(GetType(MyFixedLabelItem))
'			#End Region ' #1
			layoutControl1.ShowCustomizationForm()
		End Sub
	End Class

	#Region "#2"
	' The custom 'fixed' item.
	Public Class MyFixedLabelItem
		Inherits LayoutControlItem
		Implements IFixedLayoutControlItem
		' Must return the name of the item's type
		Public Overrides ReadOnly Property TypeName() As String Implements IFixedLayoutControlItem.TypeName
			Get
				Return "MyFixedLabelItem"
			End Get
		End Property
		Private linkCore As String
		Private controlCore As Control = Nothing

		Public Property Link() As String
			Get
				Return linkCore
			End Get
			Set(ByVal value As String)
				If Link = value Then
					Return
				End If
				Me.linkCore = value
				OnLinkChanged()
			End Set
		End Property
		Private Sub label_LinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs)
			'...
		End Sub
		Public Overrides Property Text() As String
			Get
				Return Link
			End Get
			Set(ByVal value As String)
				Link = value
			End Set
		End Property
		' This method is called when the Link property is changed.
		' It assigns the new link to the embedded LinkLabel control.
		Protected Sub OnLinkChanged()
			controlCore.Text = Link
		End Sub

		' Initialize the item.
		Private Sub OnInitialize() Implements IFixedLayoutControlItem.OnInitialize
			Me.linkCore = "www.devexpress.com"
			OnLinkChanged()
			TextVisible = False
		End Sub
		' Create and return the item's control.
		Private Function OnCreateControl() As Control Implements IFixedLayoutControlItem.OnCreateControl
			Me.controlCore = New LinkLabel()
			AddHandler (CType(controlCore, LinkLabel)).LinkClicked, AddressOf label_LinkClicked
			Return controlCore
		End Function
		' Destroy the item's control.
		Private Sub OnDestroy() Implements IFixedLayoutControlItem.OnDestroy
			If controlCore IsNot Nothing Then
				RemoveHandler (CType(controlCore, LinkLabel)).LinkClicked, AddressOf label_LinkClicked
				controlCore.Dispose()
				controlCore = Nothing
			End If
		End Sub
		Private ReadOnly Property CustomizationName() As String Implements IFixedLayoutControlItem.CustomizationName
			Get
				Return "DevExpress Link"
			End Get
		End Property
		Private ReadOnly Property CustomizationImage() As Image Implements IFixedLayoutControlItem.CustomizationImage
			Get
				Return Nothing
			End Get
		End Property
		Private ReadOnly Property AllowChangeTextLocation() As Boolean Implements IFixedLayoutControlItem.AllowChangeTextLocation
			Get
				Return False
			End Get
		End Property
		Private ReadOnly Property AllowChangeTextVisibility() As Boolean Implements IFixedLayoutControlItem.AllowChangeTextVisibility
			Get
				Return False
			End Get
		End Property
		Private ReadOnly Property AllowClipText() As Boolean Implements IFixedLayoutControlItem.AllowClipText
			Get
				Return False
			End Get
		End Property
		Private Property Owner() As ILayoutControl Implements IFixedLayoutControlItem.Owner
			Get
				Return MyBase.Owner
			End Get
			Set(ByVal value As ILayoutControl)
				MyBase.Owner = value
			End Set
		End Property
	End Class

	' Specifies which properties to display in the Property Grid
	Public Class MyFixedLabelPropertiesWrapper
		Inherits BasePropertyGridObjectWrapper
		Protected ReadOnly Property Label() As MyFixedLabelItem
			Get
				Return TryCast(WrappedObject, MyFixedLabelItem)
			End Get
		End Property
		<Description("The link's text")> _
		Public Property Link() As String
			Get
				Return Label.Link
			End Get
			Set(ByVal value As String)
				Label.Link = value
			End Set
		End Property
		Public Overrides Function Clone() As BasePropertyGridObjectWrapper
			Return New MyFixedLabelPropertiesWrapper()
		End Function
	End Class
	#End Region ' #2
End Namespace