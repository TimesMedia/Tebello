   M S B A M L   `   `   `  ???? O  KTebello.Presentation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Tebello.PresentationTebello.Presentation  2    ,Tebello.Presentation.AdministrationPromotion   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     4local"clr-namespace:Tebello.Presentation     $
??1000??  ? Title$  AdministrationPromotion??$	??450??$
??Auto?? ? 	Resources    promotionViewSource  
AlignRight ?? 
TargetType ?? Property ?? Value(   "Tebello.Presentation.RandConverter  lRandConverter??%N   &	        &	      &	 +     ?? ?? " ?.???  ? ??$ Right=?  .???? .@??? $??	Promotion??.???  ? ColumnDefinitions ?? $	??56* ??? $
??321* ? ? RowDefinitions ?? $??* ??? $??60 ?.+?&   System.Windows.Controls.DataGrid  -     Name$ PromotionDataGrid?? ? DataContext# [  	   AutoGenerateColumns	 .  
   EnableRowVirtualization
 .    RowDetailsVisibilityMode$ VisibleWhenSelected=?$
??auto??   CanUserDeleteRows .    CanUserAddRows .     SelectionUnit$ FullRow=?   SelectionMode$ Extended=?$
??auto??$??Stretch=?$ ?? IsSynchronizedWithCurrentItem$
 TrueY?$??2??????    RowHeaderStyle ?? :  4System.Windows.Controls.Primitives.DataGridRowHeader"  .???  ? ??
 ? ?????  ? ??$ 30????  ? ??
 ?  ????  ? ??$ 3q? ? ContextMenu ?? .@?m? -    m?Name$ MenuItemPayer??$??Insert a payer??m? -   $ MenuItemProduct??$??Insert a product??m? -   $ MenuItemPayerClear??$??Clear a payer??   Columns 0  *System.Windows.Controls.DataGridTextColumn  -   PayerColumn??,  &System.Windows.Controls.DataGridColumn   Header$ Payer??   Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto    
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding ?? *PayerSurname+ ?? Mode$ OneWay=?  -   ProductColumn??$ Product??$
 auto  .  ?? *ProductName+$ OneWay=?  -   DiscountPercentageColumn??$ Percentage discount??   	CellStyle# [ $
 Auto  ?? *DiscountPercentage+ ?? 	Converter# [ 4  .System.Windows.Controls.DataGridTemplateColumn  -   StartDateColumn??$ 
Start date??$
 auto    CanUserSort .    SortMemberPath$ 	StartDate??   CellTemplate ?? .R?(	  "System.Windows.Controls.DatePicker	   	  SelectedDate ?? *	StartDate+$ TwoWay=?  ?? NotifyOnValidationError  . ! ?? ValidatesOnExceptions! . " ?? UpdateSourceTrigger$" PropertyChanged=?  -	   EndDateColumn??$ End date??$
 auto  . $ EndDate?? ?? .R?	   ?? # ?? Path$# EndDate?$ TwoWay=?  . ! . $" PropertyChanged=?$ ?? ValidationRules$ 5
  /System.Windows.Controls.ExceptionValidationRule
  ?? % ?? Orientation$% 
Horizontal=?$??1??$??10q?$??2??.+??? -
   & ??Name$& 	buttonAdd??$??Add a promotion??$??	0,10,10,0q?$	??145???? -   $& buttonSubmit??$??Submit??$??	0,10,10,0q?$	??145???? -   $& 
buttonExit??$??Exit without saving??$??	0,10,10,0q?$	??145???? $??Bulletin board??.???? .+??? -   ' ??Name$' BulletinEntry??$	??407??$	??400??$??227,0q?( ?? AcceptsReturn( . ?? -   $& SaveBulletinEntry??$??80??$??0,10,0,0q?.??Save