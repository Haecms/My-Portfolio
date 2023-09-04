using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DC00_assm;
using DC00_Component;
using DC00_WinForm;
using Infragistics.Win;


namespace DC_POPUP
{
    public partial class POP_MaterialInput : BasePopupForm
    {
        string[] argument;

        #region [ 선언자 ]
        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();

        //임시로 사용할 데이터테이블 생성
        DataTable _DtTemp = new DataTable();
        private string sWorkcenterCode = string.Empty;
        private string sWorkcenterName = string.Empty;


        #endregion

        public POP_MaterialInput()
        {
            InitializeComponent();

            Common _Common = new Common();
            DataTable rtnDtTemp = _Common.Standard_CODE("PLANTCODE");  //사업장

            rtnDtTemp = Common.Get_ItemCode(new string[] { "ROH" }); // 품목코드
            Common.FillComboboxMaster(cboItemcode, rtnDtTemp);

        }

        private void POP_MaterialInput_Load(object sender, EventArgs e)
        {
            DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
            Common _Common = new Common();
            _GridUtil.InitializeGrid(this.Grid1);
            _GridUtil.InitColumnUltraGrid(Grid1, "CHK",            "선택",       GridColDataType_emu.CheckBox, 80, HAlign.Left, true, true);
            _GridUtil.InitColumnUltraGrid(Grid1, "PLANTCODE",      "공장",       GridColDataType_emu.VarChar, 120, HAlign.Left, false, false);
            _GridUtil.InitColumnUltraGrid(Grid1, "PONO",           "발주번호",   GridColDataType_emu.VarChar, 120, HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(Grid1, "ITEMCODE",       "품목",       GridColDataType_emu.VarChar, 120, HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(Grid1, "PODATE",         "발주일자",   GridColDataType_emu.VarChar, 90,  HAlign.Center, true, false);
            _GridUtil.InitColumnUltraGrid(Grid1, "POQTY",          "발주수량",   GridColDataType_emu.Integer, 130, HAlign.Right, true, false);
            _GridUtil.InitColumnUltraGrid(Grid1, "ACCEPTQTY",      "기입고수량", GridColDataType_emu.Integer, 140, HAlign.Right, true, false);
            _GridUtil.InitColumnUltraGrid(Grid1, "POSSIBLEQTY",    "입고수량",   GridColDataType_emu.Integer, 120, HAlign.Right, true, true);
            _GridUtil.InitColumnUltraGrid(Grid1, "POSEQ",          "발주순번",   GridColDataType_emu.Integer, 90,  HAlign.Center, false, false);
            _GridUtil.SetInitUltraGridBind(Grid1);

            dtStartDate.Value = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            dtEnddate.Value   = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            search();
        }


        private void search()
        {
            string sItemcode = Convert.ToString(cboItemcode.Value);
            string sPono = txtPono.Text;
            string sStartDate = string.Format("{0:yyyy-MM-dd}", dtStartDate.Value);
            string sEndDate = string.Format("{0:yyyy-MM-dd}", dtEnddate.Value);

            DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
            DBHelper helper = new DBHelper(false);
            try
            {
                rtnDtTemp = helper.FillTable("SP07_POP_MaterialInput_S1", CommandType.StoredProcedure
                , helper.CreateParameter("@PONO", sPono)
                , helper.CreateParameter("@ITEMCODE", sItemcode)
                , helper.CreateParameter("@STARTDATE", sStartDate)
                , helper.CreateParameter("@ENDDATE", sEndDate));

                Grid1.DataSource = rtnDtTemp;
            }
            catch (Exception)
            {

            }
            finally
            {
                helper.Close();
            }
        }

        private void save()
        {
            DataTable dt = Grid1.chkChange();
            if (dt == null) return;
            if (dt.Rows.Count == 0) return;

            // 저장하시겠습니까?
            if (MessageBox.Show("변경된 데이터를 저장 하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            
            DBHelper helper = new DBHelper(true);
            try
            {
                string sMessage = "";
                // 변경된 이력이 담긴 테이블에 있는 행을 하나씩 추출 
                foreach (DataRow row in dt.Rows)
                {
                    // 입고수량이 0이 들어오거나 아무 값이 안 들어왔을 경우
                    if (Convert.ToString(row["POSSIBLEQTY"]).Replace(",", "") == "0" || Convert.ToString(row["POSSIBLEQTY"]).Replace(",", "") == "")
                    {
                        continue;
                    }

                    string sPossibleQty = Convert.ToString(row["POSSIBLEQTY"]).Replace(",", "");
                    string sPoqty       = Convert.ToString(row["POQTY"]).Replace(",", "");
                    string sAcceptQty   = Convert.ToString(row["ACCEPTQTY"]).Replace(",", "");
                    string sPoseq       = Convert.ToString(row["POSEQ"]);
                    string sPodate      = Convert.ToString(row["PODATE"]);
                    if (Convert.ToInt32(sPoqty)- Convert.ToInt32(sAcceptQty) < Convert.ToInt32(sPossibleQty))
                    {
                        helper.Rollback();
                        MessageBox.Show("발주수량보다 많이 입력한 값이 있습니다. 프로그램을 종료후 다시 시도해주시기 바랍니다.");
                        return;
                    }
                    switch (row.RowState)
                    {
                        //case DataRowState.Deleted:
                        //    // 발주 내역 취소
                        //    row.RejectChanges();
                        //    helper.ExecuteNoneQuery("SP07_MM_MaterialOrderIn_D1", CommandType.StoredProcedure,
                        //                            helper.CreateParameter("@PLANTCODE", Convert.ToString(row["PLANTCODE"])),
                        //                            helper.CreateParameter("@PONO", Convert.ToString(row["PONO"])));
                        //    break;
                        //case DataRowState.Added:
                        //    // 신규 발주 등록

                        //    if (Convert.ToString(row["ITEMCODE"]) == "") sMessage += "아이템코드 ";
                        //    if (Convert.ToString(row["POQTY"]) == "") sMessage += "발주수량 ";
                        //    if (Convert.ToString(row["CUSTCODE"]) == "") sMessage += "거래처 ";
                        //    if (sMessage != "")
                        //    {
                        //        helper.Rollback();
                        //        MessageBox.Show(sMessage + "을(를) 입력해주세요");
                        //        return;
                        //    }
                        //    helper.ExecuteNoneQuery("SP07_MM_MaterialOrderIn_I1", CommandType.StoredProcedure,
                        //                            helper.CreateParameter("@PLANTCODE", Convert.ToString(row["PLANTCODE"])),
                        //                            helper.CreateParameter("@ITEMCODE", Convert.ToString(row["ITEMCODE"])),
                        //                            helper.CreateParameter("@PODATE", Convert.ToString(row["PODATE"])),
                        //                            helper.CreateParameter("@POQTY", Convert.ToString(row["POQTY"]).Replace(",", "")),
                        //                            helper.CreateParameter("@UNITCODE", Convert.ToString(row["UNITCODE"])),
                        //                            helper.CreateParameter("@CUSTCODE", Convert.ToString(row["CUSTCODE"])),
                        //                            helper.CreateParameter("@MAKER", LoginInfo.UserID));

                        //    break;
                        case DataRowState.Modified:
                            // 입고 등록을 하겠다! 라는 말
                            helper.ExecuteNoneQuery("SP07_POP_MaterialInput_U1", CommandType.StoredProcedure,
                                                    helper.CreateParameter("@PLANTCODE",   Convert.ToString(row["PLANTCODE"])), // 공장
                                                    helper.CreateParameter("@PONO",        Convert.ToString(row["PONO"])),  // 발주번호
                                                    helper.CreateParameter("@POSSIBLEQTY", sPossibleQty), // 입고수량
                                                    helper.CreateParameter("@POSEQ", sPoseq), // 입고수량
                                                    helper.CreateParameter("@PODATE", sPodate), // 입고수량
                                                    helper.CreateParameter("@INWORKER",    LoginInfo.UserID)); // 유저ID

                            break;
                    }
                    if (helper.RSCODE != "S")
                    {
                        helper.Rollback();
                        MessageBox.Show($"등록중 오류가 발생하였습니다. \r\n {helper.RSMSG}");
                        return;
                    }
                }
                helper.Commit();
                MessageBox.Show("정상적으로 등록되었습니다.");
                search(); //등록된 데이터 재 조회.
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
        private void Grid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            //this.Tag = Convert.ToString(this.Grid1.ActiveRow.Cells["ORDERNO"].Value);
            //this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            search();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            save();
            search();
        }

        private void Grid1_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            int poqty       = Convert.ToInt32(Grid1.ActiveRow.Cells["POQTY"].Value);
            int acceptqty   = Convert.ToInt32(Grid1.ActiveRow.Cells["ACCEPTQTY"].Value);
            int possibleqty = Convert.ToInt32(Grid1.ActiveRow.Cells["POSSIBLEQTY"].Value);
            if (Convert.ToString(Grid1.ActiveRow.Cells["CHK"].Text) == "1") // 체크박스를 클릭 시 발주수량 - 기발주수량이 입고수량으로 나옴
            {
                Grid1.ActiveRow.Cells["POSSIBLEQTY"].Value = Convert.ToString(poqty - acceptqty);
            }
            else if(poqty == acceptqty + possibleqty) // 목적은 체크박스 해제시 입고수량 0으로 만드는 것
            {
                Grid1.ActiveRow.Cells["POSSIBLEQTY"].Value = Convert.ToString(0);
            }
        }

        private void Grid1_AfterHeaderCheckStateChanged_1(object sender, Infragistics.Win.UltraWinGrid.AfterHeaderCheckStateChangedEventArgs e)
        {
            // 일괄 선택 시 로직
            for (int i = 0; i < Grid1.Rows.Count; i++)
            {
                if (Convert.ToString(Grid1.Rows[i].Cells["CHK"].Text) == "1")
                {
                    int poqty     = Convert.ToInt32(Grid1.Rows[i].Cells["POQTY"].Value);
                    int acceptqty = Convert.ToInt32(Grid1.Rows[i].Cells["ACCEPTQTY"].Value);
                    Grid1.Rows[i].Cells["POSSIBLEQTY"].Value = Convert.ToString(poqty - acceptqty);
                }
                else
                {
                    Grid1.Rows[i].Cells["POSSIBLEQTY"].Value = Convert.ToString(0);
                }
            }
        }

    }
}
