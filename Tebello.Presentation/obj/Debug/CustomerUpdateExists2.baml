   M S B A M L   `   `   `  ????[  WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms  5   	   O KTebello.Presentation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Subs.PresentationSubs.Presentation 5   	   -   'Subs.Presentation.CustomerUpdateExists2   5   	   Gwf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms  5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   1localclr-namespace:Subs.Presentation     5   	   -   5          ? WindowStyle$
  None=?5   7    ? Title$ CustomerUpdateExists??5   	   $??195.665??6&   $
??1150??67    ? WindowStartupLocation$ CenterScreen=?6D    ? 	Resources 5	       ?? 
TargetType ?? Property ?? Value   
AlignRight??%?   '??       &	  J     ?? 5
   
   " ??6   .?5      ??  ? ??
 ??ح?6+   5      ?? 5   
   " ?6#   .?5      ??  ? ??$ Right=?64   6D   5      5      .??5      ? $
??Auto??6   $
??Auto??6   $??Stretch=?6&   $??Stretch=?6D   $??	0,0,0,410q?6`    ? ColumnDefinitions 5   
   ?? 5      $	??31* ?6   ?? 5      $
??118* ?6   .+?5   
   ?? $
??Auto??6   $
??Auto??6   $??Stretch=?6,   $??Stretch=?6J   $??2??6f   .+?5      ?? -   5        $??textBoxInitials??5   D    ?? Left$	 107??6   
	 ?? Top$	 44??6(   $??23??68   $??57??6[   ?? 5      -   5        $??textBoxSurname??5   D   $	 295??6   $	 44??6(   $??23??68   $	??149??6Z   ?? 5      -   5        $??textBoxCellPhoneNumber??5   D   $	 593??6   $	 44??6(   $??23??68   $	??132??6b   ?? 5      -   5        $??textBoxEmailAddress??5   D   $	 887??6   $	 44??6(   $??23??68   $	??230??6_   7  1System.Windows.Forms.Integration.WindowsFormsHost  5      $	 107??6   $		 109??61   $??26??6B   $	??337??6N   
   Child.
 5      #   System.Windows.Forms.ComboBox  -   5        comboCompany??"   System.Windows.Forms.Control   Enabled .    DropDownHeight$	 300??5   C   5       ?? 5$      -   5        $??label1??5$   T   $ 18??6   $	 44??6%   $??Initials??65   $??23??6H   6T   ?? 5%      -   5        $??label2??5%   T   $	 222??6   $	 44??6&   $??Surname??66   $??23??6H    J? RenderTransformOrigin$ -1.04,0.249&?6b   ?? 5'      -	   5        $??label4??5'   Q   $	 530??6   $	 44??6&   $
??Cell??66   $??23??6E   $??45??6_   $ -1.122,0.304&?6j   ?? 5(      -
   5        $??label5??5(   R   $	 843??6   $	 44??6&   $??EMail??66   $??23??6F   6R   ?? 5)      -   5        $??label6??5)   T   $ 18??6   $		 108??6%   $??Company??66   $??28??6H   6T   ?? 5*      -   5        $??buttonCheckExisting??5*   j   $	 530??6   $		 107??6'   $!??Check for existing customer??68   $??27??6^   $	??195??6?   6?   ?? 5+      -   5        $??Cancel??5+      $	 843??6&   $		 108??69   $??27??6J   $	??274??6V   6b   5,      5/      50      