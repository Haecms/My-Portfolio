using DC_POPUP;
using DC00_assm;
using DC00_Component;
using DC00_WinForm;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Security.AccessControl;
using System.Text;
using System.Windows.Forms;
using Telerik.Reporting;
/*************************************************************************************************************
* 화면 ID  : QA_Check
* 작성자   : 조해찬
* 작성일자 : 2023-05-17
* 설명     : 원자재 생산 출고 등록
* ***********************************************************************************************************
* 수정 이력 :
* 
* 
* ***********************************************************************************************************/

namespace KDTB_FORMS
{
    public partial class QA_Check : DC00_WinForm.BaseMDIChildForm
    {
        UltraGridUtil _GridUtil = new UltraGridUtil(); // 그리드 셋팅 클래스
        public QA_Check()
        {
            InitializeComponent();
        }


        private void QA_Check_Load(object sender, EventArgs e)
        {

            #region <1번그리드 P Bad WaitingNo>
            _GridUtil.InitializeGrid(this.grid1);  
            _GridUtil.InitColumnUltraGrid(grid1, "CHK",           "선택",              GridColDataType_emu.CheckBox,    80, HAlign.Center,  true, true);
            _GridUtil.InitColumnUltraGrid(grid1, "PLANTCODE",     "공장",              GridColDataType_emu.VarChar,     80, HAlign.Left,    true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "WORKCENTERCODE","작업장",            GridColDataType_emu.VarChar,    140, HAlign.Left,    true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "MATLOTNO",      "원자재 로트",       GridColDataType_emu.VarChar,    200, HAlign.Left,    true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "BADWAITINGNO",  "판정대기수량",      GridColDataType_emu.Integer,     80, HAlign.Right,   true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMCODE",      "품목",              GridColDataType_emu.VarChar,    100, HAlign.Left,    true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMNAME",      "품목명",            GridColDataType_emu.VarChar,    100, HAlign.Left,    true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "MAKER",         "작업자",            GridColDataType_emu.VarChar,    100, HAlign.Left,    true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "MAKEDATE",      "생산실적 등록일시", GridColDataType_emu.DateTime24, 180, HAlign.Left,    true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "TOTALNO"  ,     "총 제품수량",       GridColDataType_emu.Integer,    180, HAlign.Left,    false, false);
            _GridUtil.SetInitUltraGridBind(grid1);
            #endregion

            #region <2번그리드 P_Combine> 
            _GridUtil.InitializeGrid(this.grid2);  
            _GridUtil.InitColumnUltraGrid(grid2, "CHKCODE",       "불량코드",         GridColDataType_emu.VarChar,     180, HAlign.Left,    true, false);
            _GridUtil.InitColumnUltraGrid(grid2, "BADWAITINGNO",  "총 대기수량",     GridColDataType_emu.Integer,      80, HAlign.Right,   true, false);
            _GridUtil.InitColumnUltraGrid(grid2, "MAKER",         "품질관리자",       GridColDataType_emu.VarChar,     100, HAlign.Left,    true, false);
            _GridUtil.InitColumnUltraGrid(grid2, "MAKEDATE",      "생성일시",         GridColDataType_emu.DateTime24,  180, HAlign.Left,    true, false);
            _GridUtil.InitColumnUltraGrid(grid2, "TOTALNO"  ,     "총 제품수량",       GridColDataType_emu.Integer,    180, HAlign.Left,    false, false);
            _GridUtil.SetInitUltraGridBind(grid2);
            #endregion


            #region <3번그리드 P_Lot>
            _GridUtil.InitializeGrid(this.grid3);
            _GridUtil.InitColumnUltraGrid(grid3, "CHKCODE",       "불량코드",         GridColDataType_emu.VarChar,     180,  HAlign.Left,   true, true);
            _GridUtil.InitColumnUltraGrid(grid3, "PASSLOTNO",     "로트번호", GridColDataType_emu.VarChar,     200,  HAlign.Left,   true, false);
//            _GridUtil.InitColumnUltraGrid(grid3, "NOPASSLOTNO",   "재판정 폐기 로트", GridColDataType_emu.VarChar,     200,  HAlign.Left,   true, false);
            _GridUtil.InitColumnUltraGrid(grid3, "PASSNO",        "수량",        GridColDataType_emu.Integer,      80,  HAlign.Right,  true, false);
            _GridUtil.InitColumnUltraGrid(grid3, "C_STATE",       "상태",        GridColDataType_emu.VarChar,      80,  HAlign.Left,  true, false);
            _GridUtil.InitColumnUltraGrid(grid3, "PASSREASON",    "사유",        GridColDataType_emu.VarChar,     160,  HAlign.Left,   true, false);
//            _GridUtil.InitColumnUltraGrid(grid3, "NOPASSNO",      "폐기 수량",        GridColDataType_emu.Integer,      80,  HAlign.Right,  true, false);
//            _GridUtil.InitColumnUltraGrid(grid3, "NOPASSREASON",  "폐기 사유",        GridColDataType_emu.VarChar,     160,  HAlign.Left,   true, false);
//            _GridUtil.InitColumnUltraGrid(grid3, "MAKER",         "품질관리자",       GridColDataType_emu.VarChar,     100, HAlign.Left,    true, false);
//            _GridUtil.InitColumnUltraGrid(grid3, "MAKEDATE",      "확정 일시",        GridColDataType_emu.DateTime24,  180, HAlign.Left,    true, false);
            _GridUtil.SetInitUltraGridBind(grid3);
            #endregion



            DataTable dtTemp = new DataTable();

            dtTemp = Common.StandardCODE("PLANTCODE");  // 사업장
            Common.FillComboboxMaster(cboPlantCode, dtTemp);
            UltraGridUtil.SetComboUltraGrid(grid1, "PLANTCODE", dtTemp);

            dtTemp = Common.Get_ItemCode(new string[] { "FERT" });
            Common.FillComboboxMaster(cboItemCode, dtTemp);

//            dtTemp = Common.StandardCODE("UNITCODE");   // 단위
//            UltraGridUtil.SetComboUltraGrid(grid1, "UNITCODE", dtTemp);

            dtTemp = Common.StandardCODE("ERRORTYPE");
            Common.FillComboboxMaster(cboxNoPassReason, dtTemp);



        }
        public override void DoInquire()
        {
            DBHelper helper = new DBHelper(false);
            try
            {
                base.DoInquire();   // 이거 왜 넣음???
                _GridUtil.Grid_Clear(grid1);    // 그리드 데이터 행 삭제 

                                string sPlantCode = DBHelper.nvlString(cboPlantCode.Value);   // 공장      왜 Convert.ToString() 안 씀?!??!?
                                string sLotNo     = DBHelper.nvlString(txtLotNo.Text);            // 발주번호
                                string sChkCode   = DBHelper.nvlString(txtBadCode.Text);        // 불량코드
                                string sItemCode  = DBHelper.nvlString(cboItemCode.Value);     // 품목 코드
                //                string sStartDate = string.Format("{0:yyyy-MM-dd}", dtpStart.Value);
                //                string sEndDate = string.Format("{0:yyyy-MM-dd}", dtpEnd.Value);


                // Database에서 작업자 정보 조회
                DataTable dtTemp = new DataTable();
                dtTemp = helper.FillTable("SP07_QA_Check_S1", CommandType.StoredProcedure
                                         , helper.CreateParameter("@PLANTCODE", sPlantCode)
                                         , helper.CreateParameter("@MATLOTNO",  sLotNo)
                                         , helper.CreateParameter("@ITEMCODE",  sItemCode)
//                                         , helper.CreateParameter("@STARTDATE", sStartDate)
//                                         , helper.CreateParameter("@ENDDATE",   sEndDate));
);

                DataTable dtTemp2 = new DataTable();
                dtTemp2 = helper.FillTable("SP07_QA_Check_S4", CommandType.StoredProcedure
                                           , helper.CreateParameter("@CHKCODE", sChkCode));

                // ClosePrgForm(); //프로그레스 상태 창 닫기
                grid1.DataSource = dtTemp;
                grid2.DataSource = dtTemp2;


                if (grid1.Rows.Count == 0)
                {

                    ShowDialog("조회 할 데이터가 없습니다.");
                }
            }
            catch (Exception ex)
            {
                ShowDialog(ex.ToString(), DialogForm.DialogType.OK);
            }
            finally
            {
                helper.Close();
            }
        }

        private void btnCombine_Click(object sender, EventArgs e)
        {
            #region <불량코드 등록 SQL 로직>

            DataTable dtTemp = grid1.chkChange();
            if (dtTemp.Rows.Count == 0) return;

            if (ShowDialog("선택한 원자재를 불량코드로 등록 하시겠습니까") == System.Windows.Forms.DialogResult.Cancel) return;
            DBHelper helper = new DBHelper(true);
            try
            {
                int sCount = 1;
                string sChkCode = string.Empty;
                int iTCount = 0;
                DateTime? sRsMsg = null;

                foreach (DataRow dr in dtTemp.Rows)
                {
                    if (Convert.ToString(dr["CHK"]) == "0") continue;
                    iTCount += Convert.ToInt32(dr["BADWAITINGNO"]);
                    helper.ExecuteNoneQuery("SP07_QA_Check_C1_HC", CommandType.StoredProcedure
                                            , helper.CreateParameter("@COUNT",          sCount)
                                            , helper.CreateParameter("@TOTALBAD",       iTCount)
                                            , helper.CreateParameter("@SCHKCODE",       sChkCode)
                                            , helper.CreateParameter("@ITEMNAME",       Convert.ToString(dr["ITEMNAME"]))
                                            , helper.CreateParameter("@TOTALNO",        Convert.ToInt32(dr["TOTALNO"]))
                                            , helper.CreateParameter("@ITEMCODE",       Convert.ToString(dr["ITEMCODE"]))
                                            , helper.CreateParameter("@PLANTCODE",      Convert.ToString(dr["PLANTCODE"]))
                                            , helper.CreateParameter("@WORKCENTERCODE", Convert.ToString(dr["WORKCENTERCODE"]))
                                            , helper.CreateParameter("@MATLOTNO",       Convert.ToString(dr["MATLOTNO"]))
                                            , helper.CreateParameter("@P_MAKER",        Convert.ToString(dr["MAKER"]))
                                            , helper.CreateParameter("@MAKER",          LoginInfo.UserID)
                                            , helper.CreateParameter("@MAKEDATE",       Convert.ToDateTime(dr["MAKEDATE"]))
                                            , helper.CreateParameter("@BADWAITINGNO",   Convert.ToString(dr["BADWAITINGNO"])));
                    if (helper.RSCODE != "S")
                    {
                        helper.Rollback();
                        ShowDialog(helper.RSMSG);
                        return;
                    }
                    if (sChkCode == "")
                    {
                        sChkCode = helper.RSMSG;
                    }

                    sCount++;
                }
                helper.Commit();
                MessageBox.Show("정상적으로 처리 되었습니다");
                DoInquire();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                helper.Close();
            }
            #endregion


            #region <불량코드 등록시 조회 되게>
            DBHelper helper2 = new DBHelper(false);
            try
            {
                base.DoInquire();
                _GridUtil.Grid_Clear(grid2);
                string sChkCode = DBHelper.nvlString(txtBadCode.Text);

                // Database에서 작업자 정보 조회
                DataTable dtTemp2 = new DataTable();
                dtTemp2 = helper2.FillTable("SP07_QA_Check_S4", CommandType.StoredProcedure
                                            ,helper.CreateParameter("CHKCODE", sChkCode)
                );

                ClosePrgForm(); //프로그레스 상태 창 닫기
                grid2.DataSource = dtTemp2;
                grid2.Rows[grid2.Rows.Count - 1].Activated = true;
                grid2.Rows[grid2.Rows.Count - 1].Selected = true;

            }
            catch (Exception ex)
            {
                ShowDialog(ex.ToString(), DialogForm.DialogType.OK);
            }
            finally
            {
                helper2.Close();
            }
            #endregion

        }


        // 판정확정 버튼
        private void btnSave_Click(object sender, EventArgs e)
        {

            #region  < 재판정 합격 및 실패 로트 체번>
            
            string sChkcode      = grid2.ActiveRow.Cells["CHKCODE"].Value.ToString();
            int iPassNo          = Convert.ToInt32(txtPassNo.Text);
            int iNoPassNo        = Convert.ToInt32(txtNoPassNo.Text);
            string sPassReason   = txtPassReason.Text;
            string sNoPassReason = Convert.ToString(cboxNoPassReason.Value);
            
            if(iPassNo + iNoPassNo != Convert.ToInt32(grid2.ActiveRow.Cells["BADWAITINGNO"].Value))
            {
                MessageBox.Show("합격수량과 페기수량의 합이 총 대기수량과 일치하지 않습니다.");
                return;
            }
            if ((txtPassNo.Text == "0" || txtPassNo.Text == "") && txtPassReason.Text != "") sPassReason = "";
            else if (txtPassNo.Text == "") iPassNo = 0;
            if(txtNoPassNo.Text == "0" && txtNoPassNo.Text != "")
            {
                iNoPassNo = 0;
                sNoPassReason = "";
            }
            else if(txtNoPassNo.Text != "0" && sNoPassReason == "")
            {
                MessageBox.Show("귀책 사유를 선택해주시기 바랍니다.");
                return;
            }

                if (ShowDialog("불량대기 품을 확정하시겠습니까?") == System.Windows.Forms.DialogResult.Cancel) return;
            DBHelper helper = new DBHelper(true);
            try
            {
                string sChkCode = Convert.ToString(grid2.ActiveRow.Cells["CHKCODE"].Value);

                
                helper.ExecuteNoneQuery("SP07_QA_Check_C2_GH", CommandType.StoredProcedure
                                        , helper.CreateParameter("@PASSNO",       iPassNo)
                                        , helper.CreateParameter("@NOPASSNO",     iNoPassNo)
                                        , helper.CreateParameter("@CHKCODE",      sChkcode)
                                        , helper.CreateParameter("@PASSREASON",   sPassReason)
                                        , helper.CreateParameter("@NOPASSREASON", sNoPassReason)
                                        , helper.CreateParameter("@MAKER",        LoginInfo.UserID)
                                        , helper.CreateParameter("@TOTALNO",      Convert.ToString(grid2.ActiveRow.Cells["TOTALNO"].Value))
                                        );
                if (helper.RSCODE != "S")
                {
                    helper.Rollback();
                    ShowDialog(helper.RSMSG);
                    return;
                }

                helper.Commit();
                ShowDialog("정상적으로 확정을 완료하였습니다.");
                DoInquire();

                for(int i=0; i<grid2.Rows.Count; i++)
                {
                    if(sChkCode == Convert.ToString(grid2.Rows[i].Cells["CHKCODE"].Value))
                    {
                        grid2.Rows[i].Activated = true;
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                helper.Close();
            }
            #endregion



        }


        private void grid2_AfterRowActivate(object sender, EventArgs e)
        {
            #region <재판정 로트 조회 되게>
            DBHelper helper2 = new DBHelper(false);

            string sChkcode = grid2.ActiveRow.Cells["CHKCODE"].Value.ToString();

            try
            {
                _GridUtil.Grid_Clear(grid3);


                // Database에서 작업자 정보 조회
                DataTable dtTemp2 = new DataTable();
                dtTemp2 = helper2.FillTable("SP07_QA_Check_S3", CommandType.StoredProcedure
                                                    , helper2.CreateParameter("@CHKCODE", sChkcode)

                );

                ClosePrgForm(); //프로그레스 상태 창 닫기
                grid3.DataSource = dtTemp2;

            }
            catch (Exception ex)
            {
                ShowDialog(ex.ToString(), DialogForm.DialogType.OK);
            }
            finally
            {
                helper2.Close();
            }

            #endregion
        }

        private void btnFinal_Click(object sender, EventArgs e)
        {
            if (grid3.ActiveRow == null)
            {
                MessageBox.Show("로트 셀을 선택해주세요");
                return;
            }
            string sCHKCODE = Convert.ToString(grid3.ActiveRow.Cells["CHKCODE"].Value);
            int iRestNo = Convert.ToInt32(grid3.ActiveRow.Cells["PASSNO"].Value);
            POP_LOT pop_lot = new POP_LOT(sCHKCODE, iRestNo);
            pop_lot.Show();

            grid2_AfterRowActivate(null, null);

        }
    }
}