   M S B A M L   `   `   `  ����|  xSystem.Windows.Controls.DataVisualization.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35�{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting  5   	    ArduinoWPFProj   ArduinoWPFProj.PLOT   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   W SOxyPlot.Wpf, Version=2014.1.546.0, Culture=neutral, PublicKeyToken=75e952ba404cdbb0 oxyhttp://oxyplot.org/wpf  6@   �chartingToolkit{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit  5   	     � Title$
  PLOT��5   	   $	��737��6   $
��1200��6#    � WindowStartupLocation$ CenterScreen=�60    � WindowStyle$
 None=�6U   .��5
      � .+�5   
   >   8System.Windows.Controls.DataVisualization.Charting.Chart  $
��Left=�6    $	��239��6<   $��21,0,0,0q�6I   $	��Top=�6[   $
��1161��6s      Series. 5      E   ?System.Windows.Controls.DataVisualization.Charting.ColumnSeries  -   5        $��cchar1��5   +   H   BSystem.Windows.Controls.DataVisualization.Charting.DataPointSeries   DependentValuePath$ Value��69      IndependentValuePath$	 Key��6T   ?   9System.Windows.Controls.DataVisualization.Charting.Series   Title$ اندازه ضربه ��6o      IsSelectionEnabled . $��0,0,2,0q�6�      ItemsSource 6�   �� 6�   5        5   
   $
��Left=�6    $	��242��6<   $��
21,244,0,0q�6I   $	��Top=�6]   $
��1161��6u   . 5        -   5        $��cchar2��5   +   $ Value��69   $	 Key��6T   $ اندازه ضربه ��6o    . $��0,0,2,0q�6�    6�   �� 6�   5        5   
   $
��Left=�6    $	��205��6<   $��
21,491,0,0q�6I   $	��Top=�6]   $
��1161��6u   . 5        -   5        $��cchar3��5   +   $ Value��69   $	 Key��6T   $ صحت ضربه ها ��6o    . $��	0,0,2,-62q�6�   $	��161��6�   $	��Top=�6�    6�   �� 6�   5      �� 5   
   -   5        $#��شروع یک تست جدید��5      $
��Left=�6,   $��575,701,0,0q�6G   $	��Top=�6\   $	��181��6t   $��28��6�   6�   �� 5   
   $2��,:میانگین زمانی عکس العمل��6   $��RightToLeft=�64   $
��Left=�6P   $��	87,10,0,0q�6k   $	��Top=�6~   �� 5   
   -   5        $��meanamallbl��5      $�� ��6   $
��Left=�6.   $��
232,10,0,0q�6I   $	��Top=�6]   �� 5   
   -   5        $��پرینت اطلاعات��5      $
��Left=�6:   $��
562,10,0,0q�6U   $	��Top=�6i   $	��127��6�   5!      5"      