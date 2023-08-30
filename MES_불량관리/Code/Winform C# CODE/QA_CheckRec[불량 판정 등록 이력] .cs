using DC00_assm;
using DC00_PuMan;
using DC00_WinForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KDTB_FORMS
{
    public partial class QA_CheckRec : DC00_WinForm.BaseMDIChildForm
    {
        #region < MEMBER AREA >
        DataTable rtnDtTemp = new DataTable(); // 
        UltraGridUtil _GridUtil = new UltraGridUtil();  //그리드 객체 생성 
        string plantCode = LoginInfo.PlantCode;

        #endregion
        public QA_CheckRec()
        {
            InitializeComponent();
        }

        private void QA_CheckRec_Load(object sender, EventArgs e)
        {
        #region < FORM EVENTS >
            #region ▶ GRID ◀
            //_GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);
            //_GridUtil.InitColumnUltraGrid(grid1, "PLANTCODE", "공장", true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitializeGrid(this.grid1);

            //            _GridUtil.InitColumnUltraGrid(grid1, "ITEMCODE",   "품목",      GridColDataType_emu.VarChar, 120, Infragistics.Win.HAlign.Left,  true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "CHKCODE", "체크코드",     GridColDataType_emu.VarChar,    180, Infragistics.Win.HAlign.Center, false, false);
            _GridUtil.InitColumnUltraGrid(grid1, "PASSLOTNO",  "로트넘버",  GridColDataType_emu.VarChar,    180, Infragistics.Win.HAlign.Center,  true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "PASSNOREC",     "수량",      GridColDataType_emu.VarChar,    120, Infragistics.Win.HAlign.Left,  true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "PASSREASON", "사유",      GridColDataType_emu.VarChar,    140, Infragistics.Win.HAlign.Left,  true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "C_STATE",    "상태",      GridColDataType_emu.VarChar,    50,  Infragistics.Win.HAlign.Center, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "MAKER",      "등록자",    GridColDataType_emu.VarChar,    150, Infragistics.Win.HAlign.Left,  true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "MAKEDATE",   "등록일시",  GridColDataType_emu.DateTime24, 180, Infragistics.Win.HAlign.Center, true, false);
            _GridUtil.SetInitUltraGridBind(grid1);


            _GridUtil.InitializeGrid(this.grid3);
                                              
            _GridUtil.InitColumnUltraGrid(grid3, "PLANTCODE",       "공장",       GridColDataType_emu.VarChar,     120, Infragistics.Win.HAlign.Left,  true, false);
            _GridUtil.InitColumnUltraGrid(grid3, "WORKCENTERCODE",  "작업장",     GridColDataType_emu.VarChar,     140, Infragistics.Win.HAlign.Left,  true, false);
            _GridUtil.InitColumnUltraGrid(grid3, "CHKCODE",         "확인코드",   GridColDataType_emu.VarChar,     180, Infragistics.Win.HAlign.Right, true, false);
            _GridUtil.InitColumnUltraGrid(grid3, "LOTNO",           "LOTNO",      GridColDataType_emu.VarChar,     150, Infragistics.Win.HAlign.Left,  true, false);
            _GridUtil.InitColumnUltraGrid(grid3, "ITEMCODE",        "품목",       GridColDataType_emu.VarChar,     140, Infragistics.Win.HAlign.Left,  true, false);
            _GridUtil.InitColumnUltraGrid(grid3, "ITEMNAME",        "품목명",     GridColDataType_emu.VarChar,     140, Infragistics.Win.HAlign.Left,  true, false);
            _GridUtil.InitColumnUltraGrid(grid3, "BADWAITINGNO",    "등록 수량",  GridColDataType_emu.VarChar,     120, Infragistics.Win.HAlign.Left,  true, false);
            _GridUtil.InitColumnUltraGrid(grid3, "UNITCODE",        "단위",       GridColDataType_emu.Double,      100, Infragistics.Win.HAlign.Right, true, false);
            _GridUtil.InitColumnUltraGrid(grid3, "MAKER",           "등록자",     GridColDataType_emu.VarChar,     100, Infragistics.Win.HAlign.Left,  true, false);
            _GridUtil.InitColumnUltraGrid(grid3, "MAKEDATE",        "등록일시",   GridColDataType_emu.DateTime24,  180, Infragistics.Win.HAlign.Right, true, false);
            _GridUtil.SetInitUltraGridBind(grid3);

            _GridUtil.InitializeGrid(this.grid2);

            _GridUtil.InitColumnUltraGrid(grid2, "MATLOTNO",   "원자재 로트", GridColDataType_emu.VarChar, 180, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid2, "FINALLOTNO", "최종 로트",   GridColDataType_emu.VarChar, 180, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid2, "STOCKQTY",   "합격 수량",   GridColDataType_emu.VarChar, 60, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.SetInitUltraGridBind(grid2);

            #endregion

            #region ▶ COMBOBOX ◀
            rtnDtTemp = Common.StandardCODE("PLANTCODE");  // 사업장
            UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", rtnDtTemp);

            rtnDtTemp = Common.StandardCODE("ITEMTYPE");     //품목 구분
            UltraGridUtil.SetComboUltraGrid(this.grid1, "ITEMTYPE", rtnDtTemp);

            rtnDtTemp = Common.StandardCODE("UNITCODE");     // 단위
            UltraGridUtil.SetComboUltraGrid(this.grid1, "UNITCODE", rtnDtTemp);
            UltraGridUtil.SetComboUltraGrid(this.grid3, "UNITCODE", rtnDtTemp);

            rtnDtTemp = Common.StandardCODE("yesno");
            Common.FillComboboxMaster(cboxState, rtnDtTemp);

            #endregion

            #region ▶ POP-UP ◀
            BizTextBoxManager btbManager = new BizTextBoxManager();
            #endregion

            #region ▶ ENTER-MOVE ◀
            #endregion
        #endregion
        }
        public override void DoInquire()
        {
            DoFind();
        }
        private void DoFind()
        {
            DBHelper helper = new DBHelper(false);
            try
            {
                _GridUtil.Grid_Clear(grid1);
//                string sPlantCode = Convert.ToString(cboPlantCode.Value);//DBHelper.nvlString(cboPlantCode.Value);
                string sLotNo     = DBHelper.nvlString(txtLotNo.Text);
//                string sItemCode  = DBHelper.nvlString(this.txtItemCode_H.Text);
                string sStartDate = string.Format("{0:yyyy-MM-01}", dtpStart.Value);
                string sEndDate   = string.Format("{0:yyyy-MM-dd}", dtpEnd.Value);
                string sState = Convert.ToString(cboxState.Value);


                rtnDtTemp = helper.FillTable("SP07_QA_CheckRec_S1_HC", CommandType.StoredProcedure
//                                                                   , helper.CreateParameter("PLANTCODE", sPlantCode)
                                                                   , helper.CreateParameter("@LOTNO", sLotNo)
//                                                                   , helper.CreateParameter("ITEMCODE", sItemCode)
                                                                   , helper.CreateParameter("@STARTDATE", sStartDate)
                                                                   , helper.CreateParameter("@ENDDATE",   sEndDate)
                                                                   , helper.CreateParameter("@STATE",     sState)
                                                                   );
                this.ClosePrgForm();
                this.grid1.DataSource = rtnDtTemp;
                
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

        private void grid1_AfterRowActivate(object sender, EventArgs e)
        {
            if (Convert.ToString(grid1.ActiveRow.Cells["C_STATE"].Value) != "합")
            {
                _GridUtil.Grid_Clear(grid2);
                _GridUtil.Grid_Clear(grid3);
                return;
            }
            DBHelper helper = new DBHelper(false);
            try
            {
                _GridUtil.Grid_Clear(grid3);
                string sLotNo = Convert.ToString(grid1.ActiveRow.Cells["CHKCODE"].Value);


                rtnDtTemp = helper.FillTable("SP07_QA_CheckRec_S2_HC", CommandType.StoredProcedure
                                                                   , helper.CreateParameter("@CHKCODE", sLotNo)
//                                                                   , helper.CreateParameter("LOTNO", sLotNo)
                                                                   );
                this.ClosePrgForm();
                this.grid3.DataSource = rtnDtTemp;
            }
            catch (Exception ex)
            {
                ShowDialog(ex.ToString(), DialogForm.DialogType.OK);
            }
            finally
            {
                helper.Close();
            }
            DBHelper helper2 = new DBHelper(false);
            try
            {
                _GridUtil.Grid_Clear(grid2);
                string sLotNo = Convert.ToString(grid1.ActiveRow.Cells["CHKCODE"].Value);


                rtnDtTemp = helper2.FillTable("SP07_QA_CheckRec_S3_HC", CommandType.StoredProcedure
                                                                   , helper2.CreateParameter("@CHKCODE", sLotNo)
                                                                   //                                                                   , helper.CreateParameter("LOTNO", sLotNo)
                                                                   );
                this.ClosePrgForm();
                this.grid2.DataSource = rtnDtTemp;
            }
            catch (Exception ex)
            {
                ShowDialog(ex.ToString(), DialogForm.DialogType.OK);
            }
            finally
            {
                helper2.Close();
            }
        }

        private void grid3_ClickCell(object sender, Infragistics.Win.UltraWinGrid.ClickCellEventArgs e)
        {
//            DBHelper helper = new DBHelper(false);
//            try
//            {
//                _GridUtil.Grid_Clear(grid2);
//                string sLotNo = Convert.ToString(grid1.ActiveRow.Cells["CHKCODE"].Value);


//                rtnDtTemp = helper.FillTable("SP07_QA_CheckRec_S3_HC", CommandType.StoredProcedure
//                                                                   , helper.CreateParameter("@CHKCODE", sLotNo)
////                                                                   , helper.CreateParameter("LOTNO", sLotNo)
//                                                                   );
//                this.ClosePrgForm();
//                this.grid2.DataSource = rtnDtTemp;
//            }
//            catch (Exception ex)
//            {
//                ShowDialog(ex.ToString(), DialogForm.DialogType.OK);
//            }
//            finally
//            {
//                helper.Close();
//            }
        }
    }
}
