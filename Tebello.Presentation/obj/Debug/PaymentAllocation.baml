   M S B A M L   `   `   `  ????O  KTebello.Presentation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Subs.PresentationSubs.Presentation  5   	   D @Subs.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8*clr-namespace:Subs.Data;assembly=Subs.Data	Subs.Data 5   	   ;"clr-namespace:Tebello.PresentationTebello.Presentation  6A   )    #Subs.Presentation.PaymentAllocation   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   1localclr-namespace:Subs.Presentation     5   	   ;Data*clr-namespace:Subs.Data;assembly=Subs.Data     5   	   5local1"clr-namespace:Tebello.Presentation     6A     ? Title$  Payment arrangement??5   	   $	??935??6%   $	??700??62    ? WindowStartupLocation$ Manual=?6>    ? Left$ 0??6]   
 ? Top$ 20??6f    ? 	Resources 5      (   "Tebello.Presentation.DateConverter   lDateConverter(   "Tebello.Presentation.RandConverter  lRandConverter  
AlignRight ?? 
TargetType ?? Property ?? Value??%?   &	        &	      &	 &       5
   
   6     5   
   6   ?? 5   
   " ?6#   .?5      ??  ? ??$ Right=?64   6D   5      5      .??5      ??  ?? Orientation$ Vertical=?6   .+?5   
   ?? 
????ح?$??60??6-   $ 
Horizontal=?69   .+?5      ?? -   5        	 ??Name$	 labelCustomerId??5      $	??123??6-   $??10q?6;   5      &   System.Windows.Controls.DataGrid  5   
   -   5        
  Name$
 SubscriptionIssueDataGrid??5         AutoGenerateColumns .     EnableRowVirtualization .  $	??449??6r   $??0,0,0,0q?5         RowDetailsVisibilityMode$ Visible=?6$      	RowHeight$ 30Q?6G      CanUserAddRows .     CanUserDeleteRows .  $
??Auto??6?   $
??Left=?6?      Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      -   5        issueDescriptionColumn??,  &System.Windows.Controls.DataGridColumn   Header$ Issue Description??5   j      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6?   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6E   ?? *IssueDescription+6?     5      -   5        issueIdColumn??$ Issue Id??5   X   $
 Auto 6j    6<   ?? *	IssueId+6j     5      -   5        unitsLeftColumn??$ 
Units Left??5   \   $ SizeToHeader 6p    6>   ?? *	UnitsLeft+6p   4  .System.Windows.Controls.DataGridTemplateColumn  5      $ Deliver On Credit??6)      CellTemplate 5       ?? 5!      .R?5"      ??  d? IsThreeState .  $
??Auto??6?    d? 	IsChecked 6=   ??  ?? Path$ DeliverOnCredit? ?? UpdateSourceTrigger$ PropertyChanged=?6?   5#      5%        5&      -   5        unitsPerIssueColumn??$ Units Per Issue??5&   d   $
 Auto 6}    6B   ?? *UnitsPerIssue+6}     5'      -   5        unitPriceColumn??$ 
Unit Price??5'   ?   $
 Auto 6?    6>   ?? *	UnitPrice+ ?? 	Converter# [ 6?     5(      -   5        totalColumn??$ Total??5(   ~   $
 Auto 6?    6:   ?? *Total+# [ 6?     5)      -	   5        startDateColumn??$ 
Start Date??5)   >   $
 Auto 6R    6_   ?? *	StartDate+# [  5+      ?? 5-   
   -
   5        $??
buttonSave??5-   ,   $
??Save??6   $??23??6    $??75??6>   $??10q?6\   5.      50      