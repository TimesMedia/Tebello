   M S B A M L   `   `   `  ????O  KTebello.Presentation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Subs.PresentationSubs.Presentation  5   	   D @Subs.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8*clr-namespace:Subs.Data;assembly=Subs.Data	Subs.Data 5   	   ;"clr-namespace:Tebello.PresentationTebello.Presentation  6A   ,    &Subs.Presentation.SubscriptionsCapture   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   1localclr-namespace:Subs.Presentation     5   	   ;Data*clr-namespace:Subs.Data;assembly=Subs.Data     5   	   5local1"clr-namespace:Tebello.Presentation     6A   -   5        $??Window??5   	     ? Title$  Subscriptions capture??5   	   $	??726??6'   $??1138.368??64    ? WindowStartupLocation$ Manual=?6E    ? WindowState$ Normal=?6d    ? 	Resources 5       ?? 
TargetType ?? Property ?? Value   
AlignRight+   %Tebello.Presentation.IntegerConverter  lIntegerConverter(   "Tebello.Presentation.RandConverter  lRandConverter  product2ViewSource??%$  '??       &	  n     &	 ?     &	 ?     &	 ?     ?? 5   
   " ??6   .?5	      ??  ? ??
 ????6+   ?? 5
       ? ??$ 22??6'   5      ?? 5   
   " ?6#   .?5      ??  ? ??$ Right=?64   5        5   
   6"     5   
   6   ?? 5   
   6   5      .??5      ?? -   5        $??
TabControl??5      $
??Auto??6#   $??Stretch=?60   $??Stretch=?6N   $
??Auto??6j   $??0,0,-0,0q?6x   .@?5   
   ?? $??Select??6   .??5      ?? -   5         ??Name$ SubscriptionCanvas??5      $
??Auto??61   $??Stretch=?6>   $??Stretch=?6\   
??????? ? DataContext# [ 6?   $??0,-4,-12,-34q?6?   .+?5      ?? $	??Who??6   	 ?? Left$	 10??6(   

 ?? Top$
 25??69   $??88??6I   $	??986??6U   .??5      ?? .+?5      ?? -   5         ??Name$ textReceiverSurname??5   "   $??23??6?   $	??270??6M   $		 201??6Y   $
 5??6k    ?? 
IsReadOnly . ??5      ??  ?? Path$ ReceiverSurname? ?? Mode$ OneWay=??? 5      -   5        $ textPayerSurname??5   "   $??23??6<   $	??270??6J   $		 201??6V   $
 33??6h    . ??6?   ?? $ PayerSurname?$ OneWay=??? 5      -   5        $ textReceiverCompany??5   "   $??23??6?   $	??231??6M   $		 508??6Y   $
 5??6k    . ??6?   ?? $ ReceiverCompany?$ OneWay=??? 5       -   5        $ textPayerCompany??5    "   $??23??6=   $	??231??6J   $		 508??6V   $
 33??6h    . ??6?   ?? $ PayerCompany?$ OneWay=??? 5!      -   5         ??Name$ buttonReceiver??5!   !   $??Select a receiver??6V   $	 10??6r   $
 10??6?   $	??119??6?   ?? 5"      -	   5        $ 
buttonSame??5"   !   $??Receiver is payer??6N   $	 10??6j   $
 37??6{   $	??119??6?   ?? 5#      -
   5        $ buttonPayer??5#   !   $??Select another payer??6Q   $		 845??6p   $
 37??6?   $	??119??6?   5$      5%      ?? 5&      $??Status??6   $		 863??6+   $	
 160??6=   $	??138??6N   $	??133??6[   .??5'      ?? .+?5(      	? -   5         	?Name$ 
radioQuote??5(   &   $??Quote??6;   $	 10??6K   $
 10??6\   	? 5)      -   5        $ radioActual??5)   '   $??Actual??6<   $	 10??6N   $
 30??6_    d? 	IsChecked$
 TrueY?6o   ?? 5*      $??Order number??6    $	 10??67   $
 50??6I   ?? 5+      -   5        $ textCaptureOrderNumber??5+   "   $??23??6B   $	 10??6N   $
 80??6`   $	??101??6p   ??6|   ?? $ OrderNumber?$ TwoWay=?5,      5/      ?? 51      -   5         ??Name$ checkRenewal??51      $??Generate renewal notices??61   $		 848??6T   $	
 309??6f   $	??161??6w   $
 trueY?6?   ?? 52      -   5        $ checkAutomaticRenewal??52      $??Automatic renewal??6:   $		 848??6V   $	
 344??6h   $ falseY?6y   &   System.Windows.Controls.DataGrid  54      -   5          Name$ product2DataGrid??54         AutoGenerateColumns .     EnableRowVirtualization . $	??410??6p   $	 10??6?      RowDetailsVisibilityMode$ VisibleWhenSelected=?6?   $	
 156??6?   $	??828??6?      SelectionUnit$ CellOrRowHeader=?55         SelectionMode$ Extended=?56         
IsReadOnly .    AlternatingRowBackground
 ????   CanUserAddRows .     	RowHeight$ 30Q?57   5   ??54   }   ??    RowHeaderStyle 58      ?? 59      :  4System.Windows.Controls.Primitives.DataGridRowHeader"  6    .?5:      ??  ? ??
 ? ???6<   ?? 5;       ? ??$ 30??67   ?? 5<       ? ??
 ?  ??6<   ?? 5=       ? ??$ 3q?6A   5>         Columns 5@      0  *System.Windows.Controls.DataGridTextColumn  5A      -   5        
typeColumn??,  &System.Windows.Controls.DataGridColumn   Header$
 Type??5A   l       Width5  /System.Windows.Controls.DataGridLengthConverter$
  Auto 6{   1  +System.Windows.Controls.DataGridBoundColumn!   Binding! 6A   ?? *SubscriptionTypeString+6{     5B      -   5        mediumColumn??$ Medium??5B   p   $
  Auto 6?   ! 6C   ?? *SubscriptionMediumString+6?     5C      -   5        productNameColumn??$ Product Name??5C   h   $
  Auto 6~   ! 6H   ?? *ProductName+6~   5G      ?? 5H      -   5        $ buttonCancel??5H      $??Cancel without saving??6/   $		 848??6j   $	
 534??6|   $	??148??6?   $??30??6?   ?? 5I      -   5        $ buttonCalculate??5I      $??Calculate the basket??62   $		 848??6Q   $	
 394??6c   $	??148??6t   " J? RenderTransformOrigin$" 4.867,2&?6?   $??32??6?   ?? 5J      $??Selection options: ??6   $	 12??66   $	
 122??6G   ?? 5K      -   5        # ??Name$# comboSubscriptionType??5K      $		 332??6:   $	
 122??6L   $	??156??6]   $ ?? SelectedValuePath$	$ Key??6i   % ?? DisplayMemberPath$% Value??6?   5L      ?? 5M      -   5        $# comboSubscriptionMedium??5M      $		 588??6<   $	
 126??6N   $	??165??6_   $	$ Key??6k   $% Value??6?   5N      ?? 5O      $
??Type??6   $		 212??6'   $	
 121??69   ?? 5P      $??Medium??6   $		 517??6)   $	
 121??6;   5Q      5R      ?? 5T   
   -   5        $??Basket??5T      $??Basket??6    .??5V      ?? -   5        $??myCanvas??5V      $??0,0,2,0q?6%   .+?5X        -   5        $??
BasketGrid??5X      $	??590??6-   $
??1098??6:    .   . $ Single=?5Y      
 ????$ CellOrRowHeader=?6[    .  .  $
 10??5Z   5   $	 10??6E    5[      ?? 5\      "  6    .?5]      ??  ? ??
 ? ???6<   ?? 5^       ? ??$ 30??67   ?? 5_       ? ??$ 30??68   ?? 5`       ? ??
 ?  ??6<   ?? 5a       ? ??$ 3q?6A   5b       5d      4	  .System.Windows.Controls.DataGridTemplateColumn	  5e      $ Product??61   $	  120 6B   & 	  CellTemplate& 5f      ?? 5g   "   .R?5h   &   ?? $
??Auto??5i   )   $
??Wrap=?5k   )   ??5j   )   ?? *ProductName+5k   )   5l   #   5n        5p      -   5        UnitsPerIssue??$ UnitsPerIssue??5p   E   $
  Auto 6]   '   	CellStyle#' [  6?   ! 6j   ?? *Subscription.UnitsPerIssue+6?     5q      -   5        NumberOfIssues??$ NumberOfIssues??5q   F   $
  Auto 6_   #' [  6?   ! 6l   ?? *Subscription.NumberOfIssues+6?     5r      -   5        	UnitPrice??$ 	UnitPrice??5r   A   $
  Auto 6U   #' [  5s   I   ! 5r   b   ?? *Subscription.UnitPrice+( ?? 	Converter#( [ 5s   I     5t      -   5        DeliveryMethodString??$ DeliveryMethod??5t   L   $
  Auto 6e   ! 6r   ?? *#!Subscription.DeliveryMethodString+  5u      -   5        DeliveryCost??$ Delivery cost/unit??5u   D   $
  Auto 6a   #' [  5v   ?   ! 6-   ?? *Subscription.DeliveryCost+#( [ 6?     5w      -    5        Vat??$ Vat/unit??5w   ;   $
  Auto 6N   #' [  5x   -   ! 5w   [   ?? *Subscription.Vat+#( [ 5x   -     5y      -!   5        	FullPrice??$ 
Full price??5y   A   $
  Auto 6V   #' [  5z   -   ! 5y   c   ?? *	FullPrice+#( [ 5z   -     5{      -"   5        Discount??$ Discount??5{   @   $
  Auto 6S   #' [  5|   -   ! 5{   `   ?? *FullDiscount+#( [ 5|   -     5}      -#   5        DiscountedPrice??$ Discounted price??5}   G   $
  Auto 6b   #' [  5~   -   ! 5}   o   ?? *DiscountedPrice+#( [ 5~   -   	  5?      $ Warning??61   $	  120 6B   & 5?      ?? 5?   "   .R?5?   &   ?? $
??Auto??5?   )   $
??Wrap=?5?   )   
???  ????5?   )   ?? *	Warning+5?   )   5?   #   5?      ) ? ContextMenu) 5?      ?? 5?      .@?5?      m? -$   5        $??Change??5?   '   67   5?      5?      ?? 5?      -%   5        $ buttonSubmit??5?      $??Submit??6/   $		 880??6@   $	
 632??6R   $??75??6c   6n   5?      5?      5?      5?      