   M S B A M L   `   `   `  ????O  KTebello.Presentation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Subs.PresentationSubs.Presentation  5   	   D @Subs.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8*clr-namespace:Subs.Data;assembly=Subs.Data	Subs.Data 5   	   ;"clr-namespace:Tebello.PresentationTebello.Presentation  6A   '    !Subs.Presentation.SBBankStatement   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:Subs.Presentation     5   	   ;Data*clr-namespace:Subs.Data;assembly=Subs.Data     5   	   5local1"clr-namespace:Tebello.Presentation     6A   -   5          ? Title$  SB bank statement??5	   	   $??838.235??6#   $??1573.294??64    ? 	Resources 5      .   (Tebello.Presentation.PaymentState2String   lPaymentState2String(   "Tebello.Presentation.DateConverter  lDateConverter+   %Tebello.Presentation.IntegerConverter  lIntegerConverter(   "Tebello.Presentation.RandConverter  lRandConverter ?? 
TargetType ?? Property ?? Value  
AlignRight  Subs.Data.PaymentDoc  
paymentDoc  SBBankStatementViewSource ?? Source ?? Source??%?  &	        &	      &	 &     &	 9     '?? L     &	 ?     &	 ?     &	 ?     7    5   
   6%     5   
   6     5   
   6"     5   
   6   ?? 5   
   " ??6   .?5      ??  ? ??
 ??ح?6+   5      ?? 5   
   " ?6#   .?5      ??  ? ??$ Right=?64   6D   5        5   
   6   ?? 5   
    6A   ?? *SBBankStatement+8   5      .??5      ?? $??0,0,0,0q?6    ? DataContext# [ 6   .+?5   
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
   $		 900??6   $
 75??6"   .??62   Balance over period6G   ?? 5'   
   -   5         ??Name$ textBalanceOverPeriod??5'      $
	 1044??61   $
 75??6D   $	??132??6T   $??26??6`    ?? TextAlignment$ Right=?6l   6?   ?? 5*   
   -   5        $ buttonValidate??5*      $	 10??6)   $	
 766??6:   $??Validate the payments??6h   ?? 5+   
   -	   5        $ 
buttonPost??5+      $		 264??6%   $	
 766??67   $ ??Post the validated entries??6a   {?.  6?   ?? 5,   
   -
   5        $ buttonGenerateCashbook??5,      $		 500??61   $	
 766??6C   $??Generate a cashbook??6y   ?? 5/   
   $??}??6   $	??122??6   $		 427??6)   $
 36??6;   $??46??6K   $??80(?6V   ?? 51   
   $
??From??6   $	 10??6   $
 91??60   ?? 52   
   $??to??6   $		 229??6   $
 94??6/    J? RenderTransformOrigin$ 10.102,0.457&?6?   &   System.Windows.Controls.DataGrid  54   
   -   5          Name$ SBBankStatementDataGrid??54         AutoGenerateColumns .     EnableRowVirtualization . $	??554??6o   $	 10??6?      SelectionUnit$ CellOrRowHeader=?6?   $	
 187??55      $
??1500??6$   $ ?? IsSynchronizedWithCurrentItem$
 TrueY?61      CanUserAddRows .     CanUserDeleteRows .  ??54   |   ??    RowHeaderStyle 57      ?? 58      :  4System.Windows.Controls.Primitives.DataGridRowHeader"  6   .?59      ??  ? ??
 ? ???64   ?? 5:       ? ??$ 30??6/   ?? 5;       ? ??
 ?  ??64   ?? 5<       ? ??$ 3q?69   5=         Columns 5@      0	  *System.Windows.Controls.DataGridTextColumn	  5B      -   5        transactionDateColumn??,
  &System.Windows.Controls.DataGridColumn 
  	CellStyle# [ 5B   D    
  Header$ Payment date??6l    
  
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6?   ?? *TransactionDate+ ?? 	Converter# [ 6?   	  5C      -   5        statementNoColumn??$ Statement No??5C   `    
  Width5  /System.Windows.Controls.DataGridLengthConverter$ SizeToHeader 6v    6@   ?? *StatementNo+6v   	  5D      -   5        allocationNoColumn??$ Allocation No??5D   b   $ SizeToHeader 6y    . # [ 5E   %    5D   A   ?? *AllocationNo+5E   %   	  5F      -   5        amountColumn??$ Amount??5F   ?   $ SizeToHeader 6?    . # [ 6?    6;   ?? *Amount+# [ 6?   	  5G      -   5        referenceColumn??$ 	Reference??5G   \   $	 200 6o    .  6>   ?? *	Reference+6{   	  5H      -   5        bankPaymentMethodColumn??$	 200 5H   l   $ Bank Payment Method??6x    .  6F   ?? *BankPaymentMethod+6?   	  5I      -   5        customerIdColumn??$ Customer Id??5I   ?   $ SizeToHeader 6?   # [ 5J   %    .   5I   ?   ?? *
CustomerId+ ?? Mode$ TwoWay=?# [  ?? UpdateSourceTrigger$ 	LostFocus=?5J   M   4  .System.Windows.Controls.DataGridTemplateColumn  5K      $ Message??6)   $	 300 6:      CellTemplate 5L      ?? 5M      .R?5N      ?? $
??Auto??6&   $??3q?64     ?? TextWrapping$
  Wrap=?6?   ! ?? 
IsReadOnly! .  .??6f   ??5O   "   ?? 5P   &   " ?? Path$" Message?6.   5Q   '   .??5R   0   5S      5T      5V      	  5X      $ State??6w   $
 Auto 6?    .  6%   ?? *PaymentState+# [  6?   	  5Y      $ Payment Transaction Id??6N   $ SizeToHeader 6n    . # [ 5Z   %    5Y   %   ?? *PaymentTransactionId+5Z   %   # ? ContextMenu# 5`      ?? 5a      .@?5b      m? -   5        $??Find Customer??5b      66   m? 5c      -   5        $??Goto customer??5c      66   m? 5d      -   5        $??Override??5d      61   m? 5e      -   5        $$??Mark as transfer between banks??5e      6G   m? 5f      -   5        $)??#Mark as appying to another division??5f      6L   m? 5g      -   5        $??Mark as multiple payers??5g      6@   m? 5h      -   5        $??Mark as debitorders??5h      6<   m? 5i      -   5        $??Mark as bank fees??5i      6:   m? 5j      -   5        $ ??Mark as reversible payment??5j      6C   m? 5k      -   5        $#??Mark as incorrectly deposited??5k      6F   m? 5l      -   5        $??Subset a multiple payment??5l      6B   m? 5m      -   5        $??Mark as posted??5m      67   5n      5p      5q      5r      