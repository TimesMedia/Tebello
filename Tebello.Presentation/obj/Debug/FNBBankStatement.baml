   M S B A M L   `   `   `  ????O  KTebello.Presentation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Subs.PresentationSubs.Presentation  5   	   D @Subs.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8*clr-namespace:Subs.Data;assembly=Subs.Data	Subs.Data 5   	   ;"clr-namespace:Tebello.PresentationTebello.Presentation  6A   (    "Subs.Presentation.FNBBankStatement   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:Subs.Presentation     5   	   ;Data*clr-namespace:Subs.Data;assembly=Subs.Data     5   	   5local1"clr-namespace:Tebello.Presentation     6A   -   5          ? Title$  FNB bank statement??5	   	   $??838.235??6$   $??1246.794??65    ? 	Resources 5      .   (Tebello.Presentation.PaymentState2String   lPaymentState2String(   "Tebello.Presentation.DateConverter  lDateConverter+   %Tebello.Presentation.IntegerConverter  lIntegerConverter(   "Tebello.Presentation.RandConverter  lRandConverter ?? 
TargetType ?? Property ?? Value  
AlignRight  Subs.Data.PaymentDoc  
paymentDoc  FNBBankStatementViewSource ?? Source ?? Source??%?  &	        &	      &	 &     &	 9     '?? L     &	 ?     &	 ?     &	 ?     7    5   
   6%     5   
   6     5   
   6"     5   
   6   ?? 5   
   " ??6   .?5      ??  ? ??
 ??ح?6+   5      ?? 5   
   " ?6#   .?5      ??  ? ??$ Right=?64   6D   5        5   
   6   ?? 5   
    6B   ?? *FNBBankStatement+8   5      .??5      ?? $??0,0,0,0q?6    ? DataContext# [ 6   .+?5   
   ?? -   5         ??Name$ 
buttonLoad??5      $+??%Load a new bank statement from a file??6%   	 ?? Left$	 10??6U   

 ?? Top$
 10??6f   $	??270??6v   6?   (  "System.Windows.Controls.DatePicker  5   
   -   5        $??pickerStartDate??5      $??0,0,0,0q?6-   $	 93??6>   $
 94??6O     5   
   -   5        $??pickerEndDate??5      $??0,0,0,0q?6+   $		 282??6<   $
 95??6N   ?? 5"   
   -   5        $ buttonSelectRange??5"      $		 523??6,   $
 75??6>   $(??"Select loaded entries in the range??6n   $	??270??6?   ?? 5#   
   -   5        $ buttonNotPosted??5#      $		 523??6*   $	
 111??6<   $3??-Select loaded entries in range not posted yet??6k   $	??270??6?   ?? 5%   
   -   5        $ buttonValidate??5%      $	 10??6)   $	
 766??6:   $??Validate the payments??6h   ?? 5&   
   -   5        $ 
buttonPost??5&      $		 264??6%   $	
 766??67   $ ??Post the validated entries??6a   {?.  6?   ?? 5'   
   $??}??6   $	??122??6   $		 427??6)   $
 36??6;   $??46??6K   $??80(?6V   ?? 5)   
   $
??From??6   $	 10??6   $
 91??60   ?? 5*   
   $??to??6   $		 229??6   $
 94??6/    J? RenderTransformOrigin$ 10.102,0.457&?6?   &   System.Windows.Controls.DataGrid  5,   
   -	   5          Name$ FNBBankStatementDataGrid??5,         AutoGenerateColumns .     EnableRowVirtualization . $	??554??6p   $	 10??6?      SelectionUnit$ CellOrRowHeader=?6?   $	
 187??5-      $
??Auto??6$   $ ?? IsSynchronizedWithCurrentItem$
 TrueY?61      CanUserAddRows .     CanUserDeleteRows .  ??5,   }   ??    RowHeaderStyle 5/      ?? 50      :  4System.Windows.Controls.Primitives.DataGridRowHeader"  6   .?51      ??  ? ??
 ? ???64   ?? 52       ? ??$ 30??6/   ?? 53       ? ??
 ?  ??64   ?? 54       ? ??$ 3q?69   55         Columns 58      0	  *System.Windows.Controls.DataGridTextColumn	  5;      -
   5        transactionDateColumn??,
  &System.Windows.Controls.DataGridColumn 
  	CellStyle# [ 5;   D    
  Header$ Payment date??6l    
  
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6?   ?? *TransactionDate+ ?? 	Converter# [ 6?   	  5<      -   5        allocationNoColumn??$ Allocation No??5<   b    
  Width5  /System.Windows.Controls.DataGridLengthConverter$ SizeToHeader 6y    .  6A   ?? *AllocationNo+6?   	  5=      -   5        amountColumn??$ Amount??5=   ?   $ SizeToHeader 6?    .  6;   ?? *Amount+# [ 6?   4  .System.Windows.Controls.DataGridTemplateColumn  5?      $ 	Reference??6)   $	 250 6<    .    CellTemplate 5@      ?? 5A      .R?5B      ?? $
??Auto??6&   $??3q?64    ?? TextWrapping$
 Wrap=?6?   .??6S   ??5C   "   ?? 5D   &    ?? Path$ 	Reference?6.   5E   '   .??5F   0   5G      5H      5J      	  5M      -   5        bankPaymentMethodColumn??$ Bank Payment Method??5M   l   $ SizeToHeader 6?    .  6F   ?? *BankPaymentMethod+6?   	  5R      -   5        customerIdColumn??$ Customer Id??5R   ?   $ SizeToHeader 6?   # [ 5S   %    .  5R   ?   ?? *
CustomerId+ ?? Mode$ TwoWay=?# [  ?? UpdateSourceTrigger$ 	LostFocus=?5S   M     5U      $ Message??6)   $	 250 6:    .  5V      ?? 5W      .R?5X      ?? $
??Auto??6&   $??3q?64   $
 Wrap=?6?   .??6S   ??5Y   "   ?? 5Z   &   $ Message?6.   5[   '   .??5\   0   5]      5^      5`      	  5b      $ State??6w   $
 Auto 6?    .  6%   ?? *PaymentState+# [  6?   4  .System.Windows.Controls.DataGridCheckBoxColumn  5c      $ Posted??6D   $ SizeToHeader 6T    .  6)   ?? *Posted+6i   	  5d      $ PaymentTransactionId??6N   $ SizeToHeader 6l    . # [ 6?    6%   ?? *PaymentTransactionId+6?     ? ContextMenu  5h      ?? 5i      .@?5j      m? -   5        $??Find customer??5j      66   m? 5k      -   5        $??Goto customer??5k      66   m? 5l      -   5        $ ??Find customer by Statement??5l      6C   m? 5m      -   5        $#??Mark as incorrectly deposited??5m      6F   m? 5n      -   5        $??Mark as internal transfer??5n      6B   m? 5o      -   5        $$??Mark as transfer between banks??5o      6G   m? 5p      -   5        $??Mark as multiple payers??5p      6@   m? 5q      -   5        $??Mark as debitorders??5q      6<   m? 5r      -   5        $??Mark as bank fees??5r      6:   m? 5s      -   5        $ ??Mark as reversible payment??5s      6C   m? 5t      -   5        $??Mark as posted??5t      67   m? 5u      -   5        $??Mark as override??5u      69   m? 5v      -   5        $??Subset a multiple payment??5v      6B   5w      5y      5z      5{      