using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace Subs.Data
{
    public class ClassificationNode : BaseModel
    {
        #region Globals
        private readonly ClassificationDoc2.ClassificationRow gRow;
        private readonly ObservableCollection<ClassificationNode> gNodes = new ObservableCollection<ClassificationNode>();

        #endregion

        #region Public face

        public ClassificationNode(ClassificationDoc2.ClassificationRow pRow)
        {
            gRow = pRow;
        }

        public string Classification
        {
            get
            {
                return gRow.Classification;
            }

            set
            {
                gRow.Classification = value;
                NotifyPropertyChanged("Classification");
            }
        }

        public int ClassificationId
        {
            get
            {
                return gRow.ClassificationId;
            }
        }


        public int Level
        {
            get
            {
                try
                {
                    int lResult = 0;
                    ClassificationDoc2.ClassificationRow lCurrentRow = gRow;

                    while (lCurrentRow.ClassificationId != 1)  // Not the root
                    {
                        lResult++;
                        lCurrentRow = (ClassificationDoc2.ClassificationRow)lCurrentRow.GetParentRow("FK_Classification_Classification");
                    }

                    return lResult;
                }

                catch (Exception ex)
                {
                    //Display all the exceptions

                    Exception CurrentException = ex;
                    int ExceptionLevel = 0;
                    do
                    {
                        ExceptionLevel++;
                        ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Level", "");
                        CurrentException = CurrentException.InnerException;
                    } while (CurrentException != null);

                    return 0;
                }
            }
        }

        public int ParentId
        {
            get
            {
                return gRow.ParentId;
            }

        }


        public ObservableCollection<ClassificationNode> Nodes
        {
            get
            {
                return gNodes;
            }
        }

        public void AddChild(ClassificationNode pNode)
        {
            Nodes.Add(pNode);
        }

        #endregion
    }

    public class ClassificationData2 : BaseModel
    {
        #region Globals

        public class ClassificationPath
        {
            public string Level1 { get; set; }
            public string Level2 { get; set; }
            public string Level3 { get; set; }
            public int ClassificationId { get; set; }
        }

        private readonly Subs.Data.ClassificationDoc2 gClassificationDoc = new ClassificationDoc2();
        private readonly Subs.Data.ClassificationDoc2TableAdapters.ClassificationTableAdapter gClassificationAdapter
                = new Subs.Data.ClassificationDoc2TableAdapters.ClassificationTableAdapter();

        private struct gLookupEntry
        {
            public int ClassificationId;
            public ClassificationNode Node;
        }

        private readonly List<gLookupEntry> gLookupList = new List<gLookupEntry>();

        private readonly ObservableCollection<ClassificationNode> gTree = new ObservableCollection<ClassificationNode>();
        private ClassificationNode gNewNode;
        private ClassificationNode gCurrentNode;
        private ClassificationNode gParentLevel1;
        private ClassificationNode gParentLevel2;

        #endregion

        #region Constructor

        public ClassificationData2()
        {
            try
            {
                gClassificationAdapter.AttachConnection();
                gClassificationAdapter.Fill(gClassificationDoc.Classification);

                LoadTree();
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ClassificationData2()",
                    "ConnectionString = " + gClassificationAdapter.Connection.ConnectionString);
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                throw ex;
            }
        }

        private string LoadTree()
        {
            try
            {
                // Recreate the Tree structure, up to three levels deep.

                gTree.Clear();
                gLookupList.Clear();

                foreach (ClassificationDoc2.ClassificationRow lRow1 in gClassificationDoc.Classification)
                {
                    if (Level(lRow1) == 0) continue;  // Skip the root

                    if (Level(lRow1) == 1)
                    {
                        gNewNode = new ClassificationNode(lRow1);
                        gTree.Add(gNewNode);
                        gLookupList.Add(new gLookupEntry() { ClassificationId = gNewNode.ClassificationId, Node = gNewNode });
                        gCurrentNode = gNewNode;


                        DataRow[] lChildren1 = (DataRow[])lRow1.GetChildRows("FK_Classification_Classification");

                        if (lChildren1.Length > 0)
                        {
                            // Create level 2 nodes

                            gParentLevel1 = gCurrentNode;

                            foreach (ClassificationDoc2.ClassificationRow lRow2 in lChildren1)
                            {
                                if (Level(lRow2) == 2)
                                {
                                    gNewNode = new ClassificationNode(lRow2);
                                    gParentLevel1.AddChild(gNewNode);
                                    gLookupList.Add(new gLookupEntry() { ClassificationId = gNewNode.ClassificationId, Node = gNewNode });
                                    gCurrentNode = gNewNode;

                                    DataRow[] lChildren2 = (DataRow[])lRow2.GetChildRows("FK_Classification_Classification");

                                    if (lChildren2.Length > 0)
                                    {
                                        gParentLevel2 = gCurrentNode;
                                        foreach (ClassificationDoc2.ClassificationRow lRow3 in lChildren2)
                                        {
                                            // Create level 3 nodes
                                            if (Level(lRow3) == 3)
                                            {
                                                gNewNode = new ClassificationNode(lRow3);
                                                gParentLevel2.AddChild(gNewNode);
                                                gLookupList.Add(new gLookupEntry() { ClassificationId = gNewNode.ClassificationId, Node = gNewNode });
                                                gCurrentNode = gNewNode;
                                            }
                                        }

                                    }
                                }
                            }
                        }


                    }
                }
                return "OK";
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

                return ex.Message;
            }

        }
        

        #endregion

        #region Private utilities

        private int Level(ClassificationDoc2.ClassificationRow pRow)
        {
            try
            {
                int lResult = 0;
                ClassificationDoc2.ClassificationRow lCurrentRow = pRow;

                while (lCurrentRow.ClassificationId != 1)
                {
                    lResult++;
                    lCurrentRow = (ClassificationDoc2.ClassificationRow)lCurrentRow.GetParentRow("FK_Classification_Classification");
                }

                return lResult;
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ClassificationLevel", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return 0;
            }
        }

        private int ParentId(ClassificationDoc2.ClassificationRow pRow)
        {
            try
            {

                ClassificationDoc2.ClassificationRow lCurrentRow = (ClassificationDoc2.ClassificationRow)pRow.GetParentRow("FK_Classification_Classification");
                return lCurrentRow.ClassificationId;
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ParentId", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return 0;
            }
        }

        #endregion

        #region Public face

        public ObservableCollection<ClassificationNode> Tree
        {
            get
            {
                return gTree;
            }
        }


        public string AddNode(ClassificationNode pParentNode, string pClassification)
        {
            try
            {
                ClassificationDoc2.ClassificationRow lNewRow = gClassificationDoc.Classification.NewClassificationRow();
                lNewRow.ParentId = pParentNode.ClassificationId;
                lNewRow.Classification = pClassification;
                lNewRow.ModifiedBy = System.Environment.UserName;
                lNewRow.ModifiedOn = DateTime.Now.Date;

                gClassificationDoc.Classification.AddClassificationRow(lNewRow);
                gClassificationAdapter.Update(lNewRow);

                ClassificationNode lNewNode = new ClassificationNode(lNewRow);
                pParentNode.AddChild(lNewNode);
                gLookupList.Add(new gLookupEntry() { ClassificationId = lNewNode.ClassificationId, Node = lNewNode });

                return "OK"; 

            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "AddNode", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return ex.Message;
            }
        }

        public ClassificationNode GetNode(int pClassificationId)
        {
            try
            {
                gLookupEntry lResult = gLookupList.Find(a => a.ClassificationId == pClassificationId);
                return lResult.Node;
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "GetNode", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return null;
            }
        }

        public ClassificationPath GetClassificationPath(int pClassificationId)
        {
            ClassificationPath lClassificationPath = new ClassificationPath();
            lClassificationPath.ClassificationId = pClassificationId;

            try
            {
                gLookupEntry lCurrentEntry = gLookupList.Find(a => a.ClassificationId == pClassificationId);

                while (lCurrentEntry.Node.Level != 0)
                {

                    switch (lCurrentEntry.Node.Level)
                    {
                        case 1:
                            lClassificationPath.Level1 = lCurrentEntry.Node.Classification;
                            break;
                        case 2:
                            lClassificationPath.Level2 = lCurrentEntry.Node.Classification;
                            break;
                        case 3:
                            lClassificationPath.Level3 = lCurrentEntry.Node.Classification;
                            break;
                        default:
                            throw new Exception("There is no level with the value of: " + lCurrentEntry.Node.Level.ToString());
                    }

                    if (lCurrentEntry.Node.ParentId == 1)
                    {
                        // This is a top level node
                        break;
                    }

                    lCurrentEntry = gLookupList.Find(a => a.ClassificationId == lCurrentEntry.Node.ParentId);
                }

                return lClassificationPath;
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "GetClassificationPath", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return lClassificationPath;
            }
        }


        public List<ClassificationNode> GetDescendants(ClassificationNode pNode)
        {
            List<ClassificationNode> lList = new List<ClassificationNode>();

            if (pNode.Nodes.Count == 0)
            {
                return lList;
            }

            foreach (ClassificationNode lNode in pNode.Nodes)
            {
                lList.Add(lNode);
                lList.AddRange(GetDescendants(lNode));
            }

            return lList;
        }

        public List<int> GetFamily(int pClassificationId)
        {
            List<int> lFamily = new List<int>();

            try
            {

                //Parents

                gLookupEntry lCurrentEntry = gLookupList.Find(a => a.ClassificationId == pClassificationId);


                while (lCurrentEntry.Node.Level != 1)
                {
                    lCurrentEntry = gLookupList.Find(a => a.ClassificationId == lCurrentEntry.Node.ParentId);
                    lFamily.Add(lCurrentEntry.ClassificationId);
                    if (lCurrentEntry.Node.ParentId == 1)
                    {
                        break;
                    }
                    else
                    {
                        lCurrentEntry = gLookupList.Find(a => a.ClassificationId == lCurrentEntry.Node.ParentId);
                    }

                }

                // Descendents

                lCurrentEntry = gLookupList.Find(a => a.ClassificationId == pClassificationId);

                //List<ClassificationNode> lTest = GetDescendants(lCurrentEntry.Node);



                foreach (ClassificationNode lNode in GetDescendants(lCurrentEntry.Node))
                {
                    lFamily.Add(lNode.ClassificationId);
                }

                return lFamily;

            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "GetFamily", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return lFamily;
            }
        }

        public string Update()
        {
            try
            {
                gClassificationAdapter.Update(gClassificationDoc.Classification);
                return "OK";
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Update", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

                return ex.Message;
            }
        }

        #endregion
    }
}
