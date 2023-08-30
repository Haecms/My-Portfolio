#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : QA_Analysis
//   Form Name    : 공정 재고 입출 이력
//   Name Space   : KDTB_FORMS
//   Created Date : 2023/05
//   Made By      : DSH
//   Description  : 
// *---------------------------------------------------------------------------------------------*
#endregion

#region < USING AREA >
using System;
using System.Data;
using DC_POPUP;

using DC00_assm;
using DC00_WinForm;

using Infragistics.Win.UltraWinGrid;
#endregion

namespace KDTB_FORMS
{
    public partial class QA_Analysis : DC00_WinForm.BaseMDIChildForm
    {

        #region < MEMBER AREA  : 데이터 테이블 / 그리드 유틸 선언> 
        UltraGridUtil _GridUtil = new UltraGridUtil();  //그리드 객체 생성 
        DataTable rtnDtTemp     = new DataTable(); // 
        #endregion


        #region < CONSTRUCTOR :  INITIAL > 
        public QA_Analysis()
        {
            InitializeComponent();
        }
        #endregion


        #region < FORM EVENTS >
        private void QA_Analysis_Load(object sender, EventArgs e)
        { 
            string plantCode        = LoginInfo.PlantCode;

            #region ▶ GRID ◀

            #region <1번 그리드>
            _GridUtil.InitializeGrid(this.grid1);
            //_GridUtil.InitColumnUltraGrid(grid1, "PLANTCODE",  "공장",           GridColDataType_emu.VarChar,    100, Infragistics.Win.HAlign.Left,   true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "MAKEDATE"   ,"폐기 일자",        GridColDataType_emu.VarChar,    320, Infragistics.Win.HAlign.Left,   true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "CHKCODE"   , "불량 코드",        GridColDataType_emu.VarChar,    320, Infragistics.Win.HAlign.Left,   true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "PASSREASON" ,"폐기 사유",        GridColDataType_emu.VarChar,    200, Infragistics.Win.HAlign.Left,   true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "PASSLOTNO"  ,"폐기 LOT",         GridColDataType_emu.VarChar,    320, Infragistics.Win.HAlign.Left,   true, false);
            //_GridUtil.InitColumnUltraGrid(grid1, "ITEMNAME"   ,"품명",           GridColDataType_emu.VarChar,    100, Infragistics.Win.HAlign.Left,   true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "MAKER"      ,"품질관리자",       GridColDataType_emu.VarChar,    160, Infragistics.Win.HAlign.Left,   true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "PASSNO",     "불량 수량",        GridColDataType_emu.Integer,    160, Infragistics.Win.HAlign.Right,  true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "TOTALNO",    "총 수량 ",         GridColDataType_emu.Double,     160, Infragistics.Win.HAlign.Right,  true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "BADRATE",    "불량률",           GridColDataType_emu.VarChar,    160, Infragistics.Win.HAlign.Right,  true, false);
            _GridUtil.SetInitUltraGridBind(grid1);
            #endregion

            #region <2번 그리드>
            _GridUtil.InitializeGrid(this.grid2);
            _GridUtil.InitColumnUltraGrid(grid2, "PLANTCODE",       "공 장",       GridColDataType_emu.VarChar,    100, Infragistics.Win.HAlign.Left,   true, false);
            _GridUtil.InitColumnUltraGrid(grid2, "CHKCODE"   ,      "불량 코드",   GridColDataType_emu.VarChar,    160, Infragistics.Win.HAlign.Left,   true, false);
            _GridUtil.InitColumnUltraGrid(grid2, "WORKCENTERCODE" , "작업장",      GridColDataType_emu.VarChar,    160, Infragistics.Win.HAlign.Left,   true, false);
            _GridUtil.InitColumnUltraGrid(grid2, "MATLOTNO"  ,      "원자재 LOT",  GridColDataType_emu.VarChar,    160, Infragistics.Win.HAlign.Left,   true, false);
            _GridUtil.InitColumnUltraGrid(grid2, "ITEMCODE"   ,     "품 목",       GridColDataType_emu.VarChar,    100, Infragistics.Win.HAlign.Left,   true, false);
            _GridUtil.InitColumnUltraGrid(grid2, "ITEMNAME"   ,     "품 명",       GridColDataType_emu.VarChar,    100, Infragistics.Win.HAlign.Left,   true, false);
            _GridUtil.InitColumnUltraGrid(grid2, "MAKER"   ,        "작업자",      GridColDataType_emu.VarChar,    100, Infragistics.Win.HAlign.Left,   true, false);
            _GridUtil.InitColumnUltraGrid(grid2, "MAKEDATE",        "생성 일시",   GridColDataType_emu.VarChar,    160, Infragistics.Win.HAlign.Left,   true, false);
            _GridUtil.InitColumnUltraGrid(grid2, "CUSTCODE",        "고객 코드",   GridColDataType_emu.VarChar,    160, Infragistics.Win.HAlign.Left,   false, false);
            _GridUtil.SetInitUltraGridBind(grid2);
            #endregion

            #region <3번 그리드>
            _GridUtil.InitializeGrid(this.grid3);
            _GridUtil.InitColumnUltraGrid(grid3, "CUSTCODE",        "고객 코드",   GridColDataType_emu.VarChar,     80, Infragistics.Win.HAlign.Left,   false, false);
            _GridUtil.InitColumnUltraGrid(grid3, "CUSTNAME" ,       "거래처 ",     GridColDataType_emu.VarChar,    100, Infragistics.Win.HAlign.Left,   true, false);
            _GridUtil.InitColumnUltraGrid(grid3, "BIZREQNO"   ,     "고객 번호",   GridColDataType_emu.VarChar,    160, Infragistics.Win.HAlign.Left,   true, false);
            _GridUtil.InitColumnUltraGrid(grid3, "ASGNNAME"   ,     "대표자",      GridColDataType_emu.VarChar,    100, Infragistics.Win.HAlign.Left,   true, false);
            _GridUtil.InitColumnUltraGrid(grid3, "PHONE"    ,       "휴대 번호",   GridColDataType_emu.VarChar,    160, Infragistics.Win.HAlign.Left,   true, false);
            _GridUtil.SetInitUltraGridBind(grid3);
            #endregion

            #endregion

            #region ▶ COMBOBOX ◀
            rtnDtTemp = Common.StandardCODE("PLANTCODE");  // 사업장
            //Common.FillComboboxMaster(this.cboPlantCode, rtnDtTemp);
            UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", rtnDtTemp);

            rtnDtTemp = Common.StandardCODE("UNITCODE");     //단위
            UltraGridUtil.SetComboUltraGrid(this.grid1, "UNITCODE", rtnDtTemp);
             
            rtnDtTemp = Common.Get_ItemCode(new string[] { "ROH" , "FERT", "HALB"}); // 품목

            #endregion
           
            #region ▶ ENTER-MOVE ◀
            //cboPlantCode.Value = plantCode;
            #endregion

        }
        #endregion


        #region < TOOL BAR AREA >
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
                string sErrorCode = Convert.ToString(txtErrorCode.Text);
                string sLotNo     = Convert.ToString(txtLotNo.Text);
                string sStartdate = string.Format("{0:yyyy-MM-dd}", dtStartDate.Value);
                string sEndDate   = string.Format("{0:yyyy-MM-dd}", dtEnddate.Value);

                rtnDtTemp = helper.FillTable("SP07_QA_Analysis_S1", CommandType.StoredProcedure
                                    , helper.CreateParameter("CHKCODE",   sErrorCode)
                                    , helper.CreateParameter("PASSLOTNO", sLotNo   )
                                    , helper.CreateParameter("STARTDATE", sStartdate)
                                    , helper.CreateParameter("ENDDATE",   sEndDate )
                                    );
                 
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

        #endregion

        private void grid1_AfterRowActivate(object sender, EventArgs e)
        {
            #region <불량 LOT --> 원자재 LOT 조회 되게>
            DBHelper helper2 = new DBHelper(false);

            string sChkcode = grid1.ActiveRow.Cells["CHKCODE"].Value.ToString();

            try
            {
                _GridUtil.Grid_Clear(grid2);


                // Database에서 작업자 정보 조회
                DataTable dtTemp2 = new DataTable();
                dtTemp2 = helper2.FillTable("SP07_QA_Analysis_S2", CommandType.StoredProcedure
                                                    , helper2.CreateParameter("@CHKCODE", sChkcode)

                );

                ClosePrgForm(); //프로그레스 상태 창 닫기
                grid2.DataSource = dtTemp2;

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

        private void grid2_AfterRowActivate(object sender, EventArgs e)
        {
            #region <원자재 로트의 고객 정보 조회>
            DBHelper helper2 = new DBHelper(false);

            string sCustCode = grid2.ActiveRow.Cells["CUSTCODE"].Value.ToString();

            try
            {
                _GridUtil.Grid_Clear(grid3);


                // Database에서 작업자 정보 조회
                DataTable dtTemp2 = new DataTable();
                dtTemp2 = helper2.FillTable("SP07_QA_Analysis_S3", CommandType.StoredProcedure
                                                    , helper2.CreateParameter("@CUSTCODE", sCustCode)

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
    }
}




