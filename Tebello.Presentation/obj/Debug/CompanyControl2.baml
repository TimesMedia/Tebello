   M S B A M L   `   `   `  ????O  KTebello.Presentation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Subs.PresentationSubs.Presentation  5      D @Subs.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8*clr-namespace:Subs.Data;assembly=Subs.Data	Subs.Data 5      '    !Subs.Presentation.CompanyControl2   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:Subs.Presentation     5      ;Data*clr-namespace:Subs.Data;assembly=Subs.Data     5      $??327.809??5      $	??600??6/   $??3q?6<   
????  ?  ? 	Resources  5	      )  #System.Windows.Controls.DataGridRow ?? 
TargetType ?? Triggers P? Property P? Value ?? Property ?? Value??%?   '        ?? 5
   
   "  6    5      P? 5       ?@ 
IsSelected . . ?5      ??  ? ??
 ?  ??63   5      5      5      .??5      ? -   5        $??CompanyCanvas??5      $??0,0,0,0q?6    $
??Left=?61   $	??330??6L    ? RowDefinitions 5   
   ?? 5      $??30 ?6   ?? 5      $??30 ?6   ?? 5      $??* ?6   .+?5   
   ?? -   5        $??
textSearch??5      $??23??6$    ?? TextWrapping$
 Wrap=?60   $
??auto??6D   	 ?? 	MaxLength$	 70??6Q   ?? 5   
   -   5        $??buttonSearch??5      $??Search??6%   $??75??66   $??1??6\   &   System.Windows.Controls.DataGrid  5   
   -   5        
  Name$
 companyDataGrid??5         AutoGenerateColumns .     EnableRowVirtualization . $
??Auto??6g   $??Stretch=?6u      SelectionUnit$ FullRow=?6?   $??2??6?      SelectionMode$ Single=?5         
IsReadOnly .    RowHeaderWidth$ 30Q?6A      	RowHeight$ 60Q?6U   ??5   ?   ??    RowHeaderStyle 5      ?? 5      :  4System.Windows.Controls.Primitives.DataGridRowHeader"  6   .?5       ??  ? ?? 64   ??  ?? RelativeSource ??  ?? Mode$ FindAncestor=? ?? AncestorType$ DataGridRowL? ?? AncestorLevel$ 1?? ?? Path$ 
Background??? 5!       ? ??$ 30??6/   ?? 5"       ? ??
 ?  ??64   ?? 5#       ? ??$ 3q?69   5$         Columns 5&      4  .System.Windows.Controls.DataGridTemplateColumn  5'         CellTemplate 5(      ?? 5)      .R?5*      ?? .+?5+   "   ?? 
????ح?$??30??6?   ??5,   &   ?? 5-   *   ??64   ?? *CompanyName+5/   #   ?? 51   "   
???????$??30??6;   ??52   &   ?? 53   *    ?? Orientation$ 
Horizontal=?65   .+?54   .   ?? ??68   ?? *VatRegistration+?? 55   .   $	?? | ??68   ?? 56   .   ??68   ?? *VendorNumber+?? 57   .   $	?? | ??68   ?? 58   .   ??68   ?? *CompanyRegistrationNumber+59   +   5;   #   5=      5>      5@       ? ContextMenu 5B      ?? 5C      -   5        $??gContext??5C      
 ? Tag$ AuthorityHigh??6.   .@?5D      m? -   5        $
??Edit??5D      6-   m? 5E      -   5        $	??Add??5E      6,   5F      5H      5J      5K      