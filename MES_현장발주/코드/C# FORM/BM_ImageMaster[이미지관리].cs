using DC00_assm;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace KDTB_FORMS
{
    
    public partial class BM_ImageMaster : DC00_WinForm.BaseMDIChildForm
    {
        string gItemcode = ""; // 삭제를 누르면 선택된 셀의 ITEMCODE를 받아옴, 다른 셀을 클릭하면 빈값이 되고, 클릭안하고 저장하면 ITEMCODE를 바탕으로 이미지삭제
        UltraGridUtil _GridUtil = new UltraGridUtil();
        public BM_ImageMaster()
        {
            InitializeComponent();
        }

        private void BM_ImageMaster_Load(object sender, EventArgs e)
        {
            _GridUtil.InitializeGrid(this.grid1);
            _GridUtil.InitColumnUltraGrid(grid1, "PLANTCODE",  "공장"    , GridColDataType_emu.VarChar    , 130, HAlign.Left,   true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMTYPE" ,  "품목구분", GridColDataType_emu.VarChar    , 130, HAlign.Left,   true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMCODE" ,  "품목코드", GridColDataType_emu.VarChar    , 130, HAlign.Left,   true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMNAME"  , "품목명"  , GridColDataType_emu.VarChar    , 130, HAlign.Left,   true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "UNITCODE"  , "기본단위", GridColDataType_emu.VarChar    , 130, HAlign.Left,   true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMSPEC" ,  "규격"    , GridColDataType_emu.VarChar,     130, HAlign.Center, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "EDITOR"   ,  "수정자"  , GridColDataType_emu.VarChar    , 130, HAlign.Center, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "EDITDATE" ,  "수정일자", GridColDataType_emu.DateTime24 , 130, HAlign.Left,   true, false);
            _GridUtil.SetInitUltraGridBind(grid1);                                                                              

            DataTable dtTemp = new DataTable();
            dtTemp = Common.StandardCODE("PLANTCODE");                      
            Common.FillComboboxMaster(cboPlantCode, dtTemp);
            UltraGridUtil.SetComboUltraGrid(grid1, "PLANTCODE", dtTemp);

            dtTemp = Common.StandardCODE("ITEMTYPE");
            Common.FillComboboxMaster(cboItemType, dtTemp);

            dtTemp = Common.StandardCODE("UNITCODE");
            UltraGridUtil.SetComboUltraGrid(grid1, "UNITCODE", dtTemp);


        }
        public override void DoInquire()
        {
            string sPlantCode = Convert.ToString(cboPlantCode.Value);
            DBHelper helper = new DBHelper();
            try
            {
                _GridUtil.Grid_Clear(grid1);
                DataTable dtTemp = helper.FillTable("SP07_BM_ImageMaster_S1", CommandType.StoredProcedure,
                                                            helper.CreateParameter("@PLANTCODE", Convert.ToString(cboPlantCode.Value)),
                                                            helper.CreateParameter("@ITEMNAME", Convert.ToString(txtItemName.Text)),
                                                            //helper.CreateParameter("@STOPNAME" , Convert.ToString(txtWorkerName.Text)),
                                                            helper.CreateParameter("@ITEMTYPE", Convert.ToString(cboItemType.Value)));
                grid1.DataSource = dtTemp;
                if(grid1.Rows.Count == 0)
                {
                    ClosePrgForm();
                    ShowDialog("조회할 데이터가 없습니다.");
                }
            }
            catch (Exception ex)
            {
                ShowDialog(ex.ToString());
            }
            finally
            {
                helper.Close();
            }
        }
        public override void DoDelete()
        {
            // 이미지 삭제 (이미지 컬럼에 등록된 데이터를 NULL 처리 하도록 UPDATE)
            if (grid1.Selected.Cells.Count == 0) { return; }

            picItem.Image = null;
            gItemcode = Convert.ToString(grid1.ActiveRow.Cells["ITEMCODE"].Value);
            //string sItemCode = Convert.ToString(dgtGrid.CurrentRow.Cells["ITEMCODE"].Value);
            //string sDelete = $"UPDATE TB_ItemMaster SET ITEMIMAGE = NULL WHERE ITEMCODE = '{sItemCode}'";
            //ExcuteNonQuery(sDelete);
        }
        public override void DoSave()
        {
            if (grid1.Selected.Cells.Count == 0) { return; } // 조회 후 바로 저장 눌렀을 때 또는 그냥 저장 눌렀을 때

            // 이미지를 저장하고 싶을 때
            if (gItemcode == "" && picItem.Tag != null)
            {
                DBHelper helper = new DBHelper(true);

                try
                {
                    FileStream stream = new FileStream(Convert.ToString(picItem.Tag), // 파일의 위치 정보로
                                   FileMode.Open,                 // 접근하여
                                   FileAccess.Read);              // 읽기 전용으로 가져오겠다.

                    BinaryReader reader = new BinaryReader(stream);                   // 스트림은 연결 / 바이너리 리더가 스트림 경로로 가서 2진 데이터로 RAM에 가져옴

                    Byte[] bImage = reader.ReadBytes(Convert.ToInt32(stream.Length)); // 2진데이터를 Byte 형식으로 RAM에 패킹

                    // 데이터를 메모리에 등록 완료 하였으므로  바이너리 리더와 파일스트림 종료
                    reader.Close();
                    stream.Close();

                    string sItemCode  = Convert.ToString(grid1.ActiveRow.Cells["ITEMCODE"].Value);
                    string sPlantCode = Convert.ToString(grid1.ActiveRow.Cells["PLANTCODE"].Value);
                    helper.ExecuteNoneQuery("SP07_BM_ImageMaster_U1", CommandType.StoredProcedure,
                                                        helper.CreateParameter("@ITEMCODE",  sItemCode)
                                                       ,helper.CreateParameter("@PLANTCODE", sPlantCode)
                                                       ,helper.CreateParameter("@EDITOR",    LoginInfo.UserID)
                                                       ,helper.CreateParameter("@IMAGE",     bImage));
                    helper.Commit();
                    MessageBox.Show("정상적으로 저장되었습니다.");
                }
                catch (Exception ex)
                {
                    helper.Rollback();
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    helper.Close();
                }
            }// 삭제하고 싶을 때
            else if(gItemcode != "" && picItem.Tag == null)
            {
                DBHelper helper = new DBHelper(true);
                try
                {
                    Byte[] bImage = null;
                    string sItemCode  = Convert.ToString(grid1.ActiveRow.Cells["ITEMCODE"].Value);
                    string sPlantCode = Convert.ToString(grid1.ActiveRow.Cells["PLANTCODE"].Value);
                    helper.ExecuteNoneQuery("SP07_BM_ImageMaster_D1", CommandType.StoredProcedure,
                                                        helper.CreateParameter("@ITEMCODE",  sItemCode)
                                                       ,helper.CreateParameter("@EDITOR",    LoginInfo.UserID)
                                                       ,helper.CreateParameter("@PLANTCODE", sPlantCode));
                    gItemcode  = "";
                    helper.Commit();
                    MessageBox.Show("정상적으로 저장되었습니다.");
                }
                catch (Exception ex)
                {
                    helper.Rollback();
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    helper.Close();
                }
            } 

        }

        private void btnPicLoad_Click(object sender, EventArgs e)
        {
            // 파일 탐색기를 통해서 원하는 이미지를 가져와 픽쳐박스에 표현하기.
            if (grid1.Selected.Cells.Count == 0) { return; }

            string sFilePath = string.Empty; // 이미지 파일의 경로를 담을 변수.

            // 파일 탐색기 호출
            OpenFileDialog Dialog = new OpenFileDialog();
            if (Dialog.ShowDialog() != DialogResult.OK) { return; }

            // 파일의 경로 받아오기
            sFilePath = Dialog.FileName;                
            picItem.Tag = sFilePath;                    
            picItem.Image = Bitmap.FromFile(sFilePath); 
                                                        
        }

        private void grid1_ClickCell(object sender, ClickCellEventArgs e)
        {
            DataTable rtnDtTemp = new DataTable();
            DBHelper helper = new DBHelper();
            picItem.Tag = null; // 다른 셀을 클릭하면 저장되어 있던 태그값이 없어짐
            gItemcode = "";
            try
            {
                string sItemCode  = Convert.ToString(grid1.ActiveRow.Cells["ITEMCODE"].Value);
                string sPlantCode = Convert.ToString(grid1.ActiveRow.Cells["PLANTCODE"].Value);
                rtnDtTemp = helper.FillTable("SP07_BM_ImageMaster_S2", CommandType.StoredProcedure,
                                                                       helper.CreateParameter("@ITEMCODE", sItemCode)
                                                                      ,helper.CreateParameter("@PLANTCODE", sPlantCode));



                // picItem.Image = null; ************************** 여기에 이렇게 해놓으면 미리 null이 됨. 만약 있다면 밑의 흐름을 타기에 이미지는 불러오기 가능함
                if (rtnDtTemp.Rows.Count == 0)
                {
                    picItem.Image = null;
                    return;
                }
                if (Convert.ToString(rtnDtTemp.Rows[0]["IMGSRC"]) == "")
                {
                    picItem.Image = null;
                    return;     // NULL처리된 걸 받아올 때 사용
                }


                // 이미지를 표현. (현재 dtTemp는 RAM에 있음. -> BITMAP 형식으로 변형해야함 -> 픽쳐박스의 이미지 속성에 넣어야 함)
                Byte[] bImages = (byte[])rtnDtTemp.Rows[0]["IMGSRC"];

                if (bImages == null)
                {
                    MessageBox.Show("이미지 형식으로 변형할 수 없는 데이터가 등록되어있습니다.");
                    return;
                }
                picItem.Image = new Bitmap(new MemoryStream(bImages));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                helper.Close();
            }
        }
    }
}
