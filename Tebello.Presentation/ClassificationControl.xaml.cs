using Subs.Data;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Subs.Presentation
{
    public partial class ClassificationControl : UserControl
    {
        public ClassificationData2 gClassificationData;

        public ClassificationControl()
        {
            try
            {
                InitializeComponent();
                gClassificationData = new ClassificationData2();
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), " ClassificationData2", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in ClassificationControl_Loaded: " + ex.Message);
            }
        }

        private void ClassificationControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ClassificationTreeView.ItemsSource = gClassificationData.Tree;
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), " ClassificationControl_Loaded", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show("Error in ClassificationControl_Loaded: " + ex.Message);
            }
        }

        private void Click_AddClassification(object sender, RoutedEventArgs e)
        {
            try
            {
                ElicitString lElicit = new ElicitString("What do you want to call this node?");
                lElicit.ShowDialog();

                MenuItem lMenuItem = (MenuItem)sender;

                ClassificationNode lNode = (ClassificationNode)lMenuItem.DataContext;

                if (lNode.Level > 2)
                {
                    MessageBox.Show("I support only 3 levels in the hierarchy");
                    return;
                }


                {
                    string lResult;

                    if ((lResult = gClassificationData.AddNode(lNode, lElicit.Answer)) != "OK")
                    {
                        MessageBox.Show(lResult);
                        return;
                    }
                }
                
                ClassificationTreeView.UpdateLayout();

                TreeViewItem lItem;
                lItem = (TreeViewItem)ClassificationTreeView.ItemContainerGenerator.ContainerFromItem(lNode);
                if (lItem != null)
                {
                    // It seems as though a parent is picked up only if it is on the first level.
                    lItem.ExpandSubtree();
                }
                
  
                MessageBox.Show("Done");
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Click_AddClassification", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
        }

        private void Click_RenameClassification(object sender, RoutedEventArgs e)
        {
            try
            {
                ClassificationNode lNode = (ClassificationNode)ClassificationTreeView.SelectedItem;

                ElicitString lElicit = new ElicitString("What do you want to call this node?");
                lElicit.ShowDialog();
                lNode.Classification = lElicit.Answer;
                gClassificationData.Update();
                MessageBox.Show("Done");
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Click_RenameClassification", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                MessageBox.Show(ex.Message);
            }
        }

        public int SelectedClassificationId
        {
            get
            {
                ClassificationNode lNode = (ClassificationNode)ClassificationTreeView.SelectedItem;

                if (lNode == null)
                {
                    return 0;
                }
                else
                {
                    return lNode.ClassificationId;
                }
            }
        }


    }
}
