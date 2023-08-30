using System;
using System.Data;
using System.Windows.Forms;
using Infragistics.Win;

using DC00_assm;
using DC00_PuMan;
using DC00_Component;


namespace DC_POPUP
{
    public partial class POP_LOT : DC00_WinForm.BasePopupForm
    {
        string[] argument;


        #region [ 선언자 ]
        //그리드 객체 생성
        UltraGridUtil _GridUtil = new UltraGridUtil();


        //비지니스 로직 객체 생성
        PopUp_Biz _biz = new PopUp_Biz();

        //임시로 사용할 데이터테이블 생성
        DataTable rtnDtTemp = new DataTable(); // 
        string schkcode = string.Empty;
        int iRestNo = 0;
        #endregion

        public POP_LOT(string sCHKCODE, int iRestno)
        {

            InitializeComponent();
            schkcode = sCHKCODE;
            iRestNo = iRestno;

        }

        private void POP_LOT_Load(object sender, EventArgs e)
        {


            #region <그리드>
            _GridUtil.InitializeGrid(this.Grid1);

            _GridUtil.InitColumnUltraGrid(Grid1, "PLANTCODE",      "공장", GridColDataType_emu.VarChar, 80, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(Grid1, "WORKCENTERCODE", "작업장", GridColDataType_emu.VarChar, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(Grid1, "SEQ",            "순서", GridColDataType_emu.VarChar, 80, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(Grid1, "CHKCODE",  "불량코드", GridColDataType_emu.VarChar, 150, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(Grid1, "ITEMCODE", "품목", GridColDataType_emu.VarChar, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(Grid1, "ITEMNAME", "품목명", GridColDataType_emu.VarChar, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(Grid1, "MAKER",     "작업자", GridColDataType_emu.VarChar, 100, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(Grid1, "MAKEDATE", "생산일시", GridColDataType_emu.DateTime24, 160, Infragistics.Win.HAlign.Right, true, false);
            _GridUtil.InitColumnUltraGrid(Grid1, "BADWAITINGNO", "남은수량", GridColDataType_emu.Integer, 160, Infragistics.Win.HAlign.Right, true, false);
            _GridUtil.InitColumnUltraGrid(Grid1, "PASSNO",   "원래수량", GridColDataType_emu.Integer, 160, Infragistics.Win.HAlign.Right, true, false);
            _GridUtil.InitColumnUltraGrid(Grid1, "MATLOTNO", "원자재 로트", GridColDataType_emu.VarChar, 160, Infragistics.Win.HAlign.Left, true, false);


            //_GridUtil.InitColumnUltraGrid(Grid1, "UNITCODE",        "단위",       GridColDataType_emu.Double,      100, Infragistics.Win.HAlign.Right, true, false);
            //_GridUtil.InitColumnUltraGrid(Grid1, "BADWAITINGNO",    "등록 수량",  GridColDataType_emu.VarChar,     120, Infragistics.Win.HAlign.Left,  true, false);
            _GridUtil.SetInitUltraGridBind(Grid1);
            #endregion


            #region <콤보박스>
            //DataTable rtnDtTemp = new DataTable(); // return DataTable 공통
            //Common _Common = new Common();
            //rtnDtTemp = _Common.Standard_CODE("CUSTTYPE");  //거래처
            //UltraGridUtil.SetComboUltraGrid(this.Grid1, "CustType", rtnDtTemp);
            #endregion

            search();

        }


        private void search()
        {
            DBHelper helper = new DBHelper(false);
            _GridUtil.Grid_Clear(Grid1);


            rtnDtTemp = helper.FillTable("SP07_QA_POP_S1", CommandType.StoredProcedure
                                        , helper.CreateParameter("@CHKCODE", schkcode)
                                         );


            this.ClosePrgForm();
            this.Grid1.DataSource = rtnDtTemp;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (Grid1.ActiveRow == null)
            {
                MessageBox.Show("합격 판정된 제품의 원자재를 선택해주세요");
                return;
            }
            if (Convert.ToString(txtInputNo.Text) == "")
            {
                MessageBox.Show("입력 수량을 입력해 주시기 바랍니다.");
                return;
            }
            if (Convert.ToInt32(txtRestNo.Text) < Convert.ToInt32(txtInputNo.Text))
            {
                string empty = "";
                MessageBox.Show("현재 합격된 수량보다 많이 입력되었습니다.");
                return;
            }

            if (Convert.ToInt32(txtInputNo.Text) == 0)
            {
                MessageBox.Show("합격 판정 수량을 입력해주세요.");
                return;
            }


            #region  < 재판정 합격 및 실패 로트 체번>

            string sPLANTCODE      = Grid1.ActiveRow.Cells["PLANTCODE"].Value.ToString();
            string sWORKCENTERCODE = Grid1.ActiveRow.Cells["WORKCENTERCODE"].Value.ToString();
            string sCHKCODE  = Grid1.ActiveRow.Cells["CHKCODE"].Value.ToString();
            string sMATLOTNO = Grid1.ActiveRow.Cells["MATLOTNO"].Value.ToString();
            int sSTOCKQTY    = Convert.ToInt32(txtInputNo.Text);
            string sITEMCODE = Grid1.ActiveRow.Cells["ITEMCODE"].Value.ToString();
            string sITEMNAME = Grid1.ActiveRow.Cells["ITEMNAME"].Value.ToString();
            string sMAKER    = LoginInfo.UserID;


            DBHelper helper = new DBHelper(true);
            try
            {
                string sChkCode = Convert.ToString(Grid1.ActiveRow.Cells["CHKCODE"].Value);


                helper.ExecuteNoneQuery("SP07_QA_POP_U1", CommandType.StoredProcedure
                                        , helper.CreateParameter("@PLANTCODE",      sPLANTCODE)
                                        , helper.CreateParameter("@WORKCENTERCODE", sWORKCENTERCODE)
                                        , helper.CreateParameter("@CHKCODE",        sCHKCODE)
                                        , helper.CreateParameter("@MATLOTNO",       sMATLOTNO)
                                        , helper.CreateParameter("@STOCKQTY",       sSTOCKQTY)
                                        , helper.CreateParameter("@ITEMCODE",       sITEMCODE)
                                        , helper.CreateParameter("@ITEMNAME",       sITEMNAME)
                                        , helper.CreateParameter("@MAKER",          sMAKER)
                                        );

                helper.Commit();
                MessageBox.Show("정상적으로 처리 되었습니다.");
                search();

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
            #endregion
        }

        private void Grid1_ClickCell(object sender, Infragistics.Win.UltraWinGrid.ClickCellEventArgs e)
        {
            txtRestNo.Text = Convert.ToString(Grid1.ActiveRow.Cells["PASSNO"].Value);
        }
    }
}