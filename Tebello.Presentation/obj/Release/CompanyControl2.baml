   M S B A M L   `   `   `  ???? O  KTebello.Presentation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Subs.PresentationSubs.Presentation  D @Subs.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8*clr-namespace:Subs.Data;assembly=Subs.Data	Subs.Data '    !Subs.Presentation.CompanyControl2   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     1localclr-namespace:Subs.Presentation     ;Data*clr-namespace:Subs.Data;assembly=Subs.Data     $??327.809??$	??600??$??3q?
????  ?  ? 	Resources  )  #System.Windows.Controls.DataGridRow ?? 
TargetType ?? Triggers P? Property P? Value ?? Property ?? Value??%R   '        ?? "   P?  ?@ 
IsSelected . . ???  ? ??
 ?  ??.??? -   $??CompanyCanvas??$??0,0,0,0q?$
??Left=?$	??330?? ? RowDefinitions ?? $??30 ??? $??30 ??? $??* ?.+??? -   $??
textSearch??$??23?? ?? TextWrapping$
 Wrap=?$
??auto??	 ?? 	MaxLength$	 70???? -   $??buttonSearch??$??Search??$??75??$??1??&   System.Windows.Controls.DataGrid  -   
  Name$
 companyDataGrid??   AutoGenerateColumns .     EnableRowVirtualization . $
??Auto??$??Stretch=?   SelectionUnit$ FullRow=?$??2??   SelectionMode$ Single=?   
IsReadOnly .    RowHeaderWidth$ 30Q?   	RowHeight$ 60Q?????    RowHeaderStyle ?? :  4System.Windows.Controls.Primitives.DataGridRowHeader"  .???  ? ?? ??  ?? RelativeSource ??  ?? Mode$ FindAncestor=? ?? AncestorType$ DataGridRowL? ?? AncestorLevel$ 1?? ?? Path$ 
Background???  ? ??$ 30????  ? ??
 ?  ????  ? ??$ 3q?   Columns 4  .System.Windows.Controls.DataGridTemplateColumn     CellTemplate ?? .R??? .+??? 
????? ?$??30?????? ???? *CompanyName+?? 
???????$??30??????  ?? Orientation$ 
Horizontal=?.+??? ???? *VatRegistration+?? $	?? | ???? ???? *VendorNumber+?? $	?? | ???? ???? *CompanyRegistrationNumber+ ? ContextMenu ?? -   $??gContext??
 ? Tag$ AuthorityHigh??.@?m? -   $
??Edit??m? -   $	??Add??