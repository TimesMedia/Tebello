   M S B A M L   `   `   `  ????O  KTebello.Presentation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Tebello.PresentationTebello.Presentation  5   	   %    Tebello.Presentation.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:Tebello.Presentation     5   	   -   5          ? WindowState$  Normal=?5   	    ? Title$ 
MainWindow??5	   	   $	??450??6   $	??800??6)   .??5
      ? $??0,0,0,0q?6   $
??Auto??6   $
??Auto??6*   $??Stretch=?67   $??Stretch=?6S    ? ColumnDefinitions 5   
   ?? 5      $??* ?6    ? RowDefinitions 5   
   ?? 5      $??* ?6   ?? 5      $??5* ?6   .+?5   
   o? -   5         o?Name$ MainMenu??5      -  'System.Windows.Input.KeyboardNavigation   TabNavigation$ Cycle=?6!   $
??Auto??6J   $
??Auto??6X   $??Stretch=?6e   $??Stretch=?6?   $
??Bold"?6?   $??1q?6?   
???????.@?5      ?? $??mims-small.png ??6   $??45??6-   $??45??69   $	??169??6G   $y?Visible=?6S   m? 5      $??Administration ??6   $	??114??60   $??Oblique$?6>   $??45??6R   $??9q?6^   .@?5      m? -   5        $??Company??5      
 ? Tag$ AuthorityHigh??5      6/   m? 5      -   5        $??Country??5      $ AuthorityHigh??6B   6V   m? 5      -   5        $??Customer classification??5      $ AuthorityHigh??6a   6u   m? 5      -   5        $??DeliveryAddress??5      $ AuthorityHigh??6R   6f   m? 5      -   5        $??DeliveryCost??5      $ AuthorityHigh??6L   6`   m? 5!      -   5         m?Name$ 	PostCodes??5!   q   $??
Post codes??6   $ AuthorityHigh??6F   6q   m? 5"      -	   5        $??Product??5"      $ AuthorityHigh??6Q   6e   m? 5#      -
   5        $??	Promotion??5#      $ AuthorityHigh??6G   6[   m? 5$      -   5        $??Refresh valid values??5$      69   5&      m? 5'      -   5        $??	Product  ??5'      $ AuthorityHigh??6*   $
??Bold"?6U   $??Oblique$?6g   $??47??6|   $??9q?6?   .@?5(      m? -   5        $??Deliver??5(      6,   m? 5)      -   5        $??Delivery reversal??5)      66   5*      m? 5,      $??Subscription??6   $??Oblique$?6.   $
??Left=?6B   $
??Bold"?6]   $??45??6o   $??9q?6{   z?5-      V? 5.      .~?5/      ?? ?? 50      ?? 51       ?? Angle$ -0.222Q?6*   T? 52      53      .@?55      m? -   5        $
??GoTo??55      6)   m? 56      -   5        $??Quote??56      6*   m? 57      -   5        $??Global_Skip??57      $ AuthorityHigh??6I   6]   58      m? 5:      -   5        $ CustomerMenu??5:      $??Customer??6-   $??Right=?6?   $
??Bold"?6[   $??Oblique$?6m   $??45??6?   $??9q?6?   .@?5;      m? -   5        $??Go to??5;      6+   m? 5<      -   5        $??Communication??5<      $ AuthorityMedium??6V   6l   m? 5=      -   5        $??SB bank statement??5=      $ AuthorityMedium??6T   6j   m? 5>      -   5        $??FNB bank statement??5>      $ AuthorityMedium??6V   6l   m? 5?      -   5        $??DO bank statement??5?      $ AuthorityMedium??6\   6r   m? 5@      -   5        $??
CreditNote??5@      $ AuthorityHigh??6H   6\   5B      m? 5D      -   5        $?? Maintenance  ??5D      $ AuthorityHigh??6/   $
??Bold"?6Z   $??Oblique$?6l   $??45??6?   $??8q?6?   .@?5E      m? -   5        $??Refresh enums??5E      62   m? 5F      -   5        $??Postcode standardisation??5F      6=   m? 5G      -   5        $"??Postcode Add Sapo Compliment??5G      6A   m? 5H      -   5        $
??Test??5H      6)   5I      5J      ? 5M   
   $??1??6   $??0,10,0,0q?6   
?????? .+?5N      ? $
??auto??6   $
??auto??6!   ? 5O      $
??auto??6   ?? 5P      -   5        	 ??Name$	 labelCurrentCustomer??5P      $??Center=?62   $??Center=?6O   $??30(?6j   6{   5Q      5R      5T      