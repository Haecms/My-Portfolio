using DC_POPUP;
using DC00_assm;
using DC00_Component;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.AccessControl;
using System.Text;
using System.Windows.Forms;
/*************************************************************************************************************
 * 화면 ID  : MM_MaterialInput
 * 작성자   : 조해찬
 * 작성일자 : 2023-05-17
 * 설명     : 구매 자재 발주 등록 및 입고 등록 (관리)
 * ***********************************************************************************************************
 * 수정 이력 :
 * 
 * 
 * ***********************************************************************************************************/

namespace KDTB_FORMS
{
    public partial class MM_MaterialInput : DC00_WinForm.BaseMDIChildForm
    {
        UltraGridUtil _GridUtil = new UltraGridUtil(); // 그리드 셋팅 클래스
        public MM_MaterialInput()
        {
            InitializeComponent();
        }

        private void MM_MaterialInput_Load(object sender, EventArgs e)
        {
            // 1. 그리드 셋팅
            
            _GridUtil.InitializeGrid(grid1);   // 싹 비우고 시작 Clear랑 같은 의미
            _GridUtil.InitColumnUltraGrid(grid1, "PLANTCODE",    "공장"    ,     GridColDataType_emu.VarChar        , 130, HAlign.Left , true, true);
            _GridUtil.InitColumnUltraGrid(grid1, "INPUTSEQ" ,    "입고번호",     GridColDataType_emu.VarChar        ,  80, HAlign.Left , true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "INPUTLOT" ,    "입고LOT"    ,  GridColDataType_emu.VarChar        , 200, HAlign.Left , true, true);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMCODE"   ,  "품목",         GridColDataType_emu.VarChar,         100, HAlign.Left , true, true);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMNAME"   ,  "품명",         GridColDataType_emu.VarChar,         100, HAlign.Left , true, true);
            _GridUtil.InitColumnUltraGrid(grid1, "INPUTQTY"    , "입고수량",     GridColDataType_emu.Double         , 100, HAlign.Right, true, true);
            _GridUtil.InitColumnUltraGrid(grid1, "INPUTDATE" ,   "입고일자"    , GridColDataType_emu.YearMonthDay,    100, HAlign.Left , true, true);
            _GridUtil.InitColumnUltraGrid(grid1, "PONO" ,        "발주번호",     GridColDataType_emu.VarChar        , 150, HAlign.Left , true, true);
            _GridUtil.InitColumnUltraGrid(grid1, "WO"   ,        "발주장소",     GridColDataType_emu.VarChar,          70, HAlign.Center,true, true);
            _GridUtil.InitColumnUltraGrid(grid1, "ORDERDATE" ,   "발주일자"    , GridColDataType_emu.YearMonthDay,    100, HAlign.Left , true, true);
            _GridUtil.InitColumnUltraGrid(grid1, "CUSTNAME"   ,  "거래처",       GridColDataType_emu.VarChar,         100, HAlign.Left , true, true);
            _GridUtil.InitColumnUltraGrid(grid1, "MAKER"    ,    "등록자"  ,     GridColDataType_emu.VarChar        , 100, HAlign.Left  , true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "MAKEDATE" ,    "등록일시",     GridColDataType_emu.DateTime24     , 150, HAlign.Center, true, false);
                                                                                
            //_GridUtil.InitColumnUltraGrid(grid1, "AORDERSTATUS", "자동발주여부", GridColDataType_emu.VarChar, 100, HAlign.Center, true, false);

            //_GridUtil.InitColumnUltraGrid(grid1, "CHK"      , "입고등록", GridColDataType_emu.CheckBox       , 80 , HAlign.Left , true, true);
            //_GridUtil.InitColumnUltraGrid(grid1, "INQTY"    , "입고수량", GridColDataType_emu.Double         , 100, HAlign.Right, true, true);
                                                                                                             
            //_GridUtil.InitColumnUltraGrid(grid1, "LOTNO"    , "LOT번호" , GridColDataType_emu.VarChar        , 150, HAlign.Left , true, false);
            //_GridUtil.InitColumnUltraGrid(grid1, "INDATE"   , "입고일자", GridColDataType_emu.VarChar        , 100, HAlign.Left , true, false);
            //_GridUtil.InitColumnUltraGrid(grid1, "INWORKER" , "입고자"  , GridColDataType_emu.VarChar        , 150, HAlign.Left , true, false);


            
            //_GridUtil.InitColumnUltraGrid(grid1, "EDITDATE" , "수정일시", GridColDataType_emu.DateTime24     , 130, HAlign.Center, true, false);
            //_GridUtil.InitColumnUltraGrid(grid1, "EDITOR"   , "수정자"  , GridColDataType_emu.VarChar        , 130, HAlign.Left  , true, false);
            _GridUtil.SetInitUltraGridBind(grid1); // 디스플레이랑 ?을 실시간으로 바인딩

            // 콤보박스 셋팅
            DataTable dtTemp = new DataTable();

            // 공장
            dtTemp = Common.StandardCODE("PLANTCODE");
            Common.FillComboboxMaster(cboPlantCode, dtTemp);
            UltraGridUtil.SetComboUltraGrid(grid1, "PLANTCODE", dtTemp);
            //UltraGridUtil.SetComboUltraGrid(grid1, "PLANTCODE", dtTemp);

            //dtTemp = Common.StandardCODE("AORDERSTATUS");
            ////Common.FillComboboxMaster(cboAOrderFlag, dtTemp);

            //// 협력업체 (거래처)
            //dtTemp = Common.GET_Cust_Code("V");                           
            ////Common.FillComboboxMaster(cboCustCode, dtTemp);               
            //UltraGridUtil.SetComboUltraGrid(grid1, "CUSTCODE", dtTemp);   

            //// 거래처
            //dtTemp = Common.StandardCODE("UNITCODE");                     
            //UltraGridUtil.SetComboUltraGrid(grid1, "UNITCODE", dtTemp);   

            //// 발주 품목
            dtTemp = Common.Get_ItemCode(new string[] { "ROH" });
            Common.FillComboboxMaster(cboItemCode, dtTemp);
            //UltraGridUtil.SetComboUltraGrid(grid1, "ITEMCODE", dtTemp);

            // 입출고 일자 기본 날짜 선택
            dtpStart.Value = string.Format("{0:yyyy-MM-01}", DateTime.Now);
            dtpEnd.Value   = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
        }

        public override void DoInquire()
        {
            // 구매자재 발주 및 입고 내역 조회
            string sPlantCode  = Convert.ToString(cboPlantCode.Value);            // 공장
            //string sCustCode   = Convert.ToString(cboCustCode.Value);             // 사번
            string sStartDate  = string.Format("{0:yyyy-MM-dd}", dtpStart.Value); // 입출 시작일자
            string sEndDate    = string.Format("{0:yyyy-MM-dd}", dtpEnd.Value);   // 입출 종료일자
            string sPono       = Convert.ToString(txtPono.Text);                  // 발주번호
            string sInputLOT   = Convert.ToString(txtInputLot.Text);
            string sItemCode   = Convert.ToString(cboItemCode.Value);             // 품목 코드
            //string sAorderFlag = Convert.ToString(cboAOrderFlag.Value);

            DBHelper helper = new DBHelper(false);
            try
            {
                _GridUtil.Grid_Clear(grid1);    // 그리드 데이터 행 삭제 
                // Database에서 작업자 정보 조회
                DataTable dtTemp = new DataTable();
                dtTemp = helper.FillTable("SP07_MM_MaterialInput_S1", CommandType.StoredProcedure
                                         //, helper.CreateParameter("@PLANTCODE", sPlantCode)
                                         //, helper.CreateParameter("@CUSTCODE" , sCustCode)
                                         , helper.CreateParameter("@STARTDATE", sStartDate)
                                         , helper.CreateParameter("@ENDDATE",   sEndDate)
                                         , helper.CreateParameter("@INPUTLOT",  sInputLOT)
                                         , helper.CreateParameter("@PLANTCODE", sPlantCode)
                                         , helper.CreateParameter("@ITEMCODE",  sItemCode)
                                         , helper.CreateParameter("@PONO",      sPono));
                                         //, helper.CreateParameter("@ITEMCODE" , sItemCode)
                                         //, helper.CreateParameter("@AORDERSTATUS", sAorderFlag));
                grid1.DataSource = dtTemp;
                //if (grid1.Rows.Count == 0)
                //{
                //    ClosePrgForm(); //프로그레스 상태 창 닫기
                //    ShowDialog("조회 할 데이터가 없습니다.");
                //}
                //for ( int i =0; i <  grid1.Rows.Count; i++)
                //{
                //    grid1.Rows[i].Cells["PONO"].Activation     = Activation.NoEdit;
                //    grid1.Rows[i].Cells["MAKEDATE"].Activation = Activation.NoEdit;
                //    grid1.Rows[i].Cells["MAKER"].Activation    = Activation.NoEdit;
                //    grid1.Rows[i].Cells["EDITDATE"].Activation = Activation.NoEdit;
                //    grid1.Rows[i].Cells["EDITOR"].Activation   = Activation.NoEdit;
                //}

            }
            catch(Exception ex)
            {
                ShowDialog(ex.Message);
            }
            finally
            {
                helper.Close();
            }
        }
        public override void DoNew()
        {
            //// 새로운 행 생성 (구매자재 발주 내역 등록을 위한 추가)
            //grid1.InsertRow();

            //// 기본 값 셋팅
            //grid1.ActiveRow.Cells["PLANTCODE"].Value = LoginInfo.PlantCode;                 // 공장 : 로그인 할 때 당시의 플랜트코드값이 셋팅됨
            //grid1.ActiveRow.Cells["PODATE"].Value    = DateTime.Now.ToString("yyyy-MM-dd");                                    // 그룹

            //// 수정 불가 컬럼 셋팅
            //grid1.ActiveRow.Cells["PONO"].Activation     = Activation.NoEdit;
            //grid1.ActiveRow.Cells["MAKEDATE"].Activation = Activation.NoEdit;
            //grid1.ActiveRow.Cells["MAKER"].Activation    = Activation.NoEdit;
            //grid1.ActiveRow.Cells["EDITDATE"].Activation = Activation.NoEdit;
            //grid1.ActiveRow.Cells["EDITOR"].Activation   = Activation.NoEdit;
        }

        public override void DoDelete()
        {
            //grid1.DeleteRow();
        }

        public override void DoSave()
        {
            //// 1. 변경된 이력이 있는 행을 데이터 테이블에 담기
            //DataTable dt = grid1.chkChange();
            //if (dt.Rows.Count == 0) return;

            //// 저장하시겠습니까?
            //if (ShowDialog("변경된 데이터를 저장 하시겠습니까?") == DialogResult.Cancel)
            //{
            //    return;
            //}

            //DBHelper helper = new DBHelper(true);
            //try
            //{
            //    string sMessage = "";
            //    // 변경된 이력이 담긴 테이블에 있는 행을 하나씩 추출 
            //    foreach (DataRow row in dt.Rows)
            //    {
            //        switch (row.RowState)
            //        {
            //            case DataRowState.Deleted:
            //                // 발주 내역 취소
            //                row.RejectChanges();
            //                helper.ExecuteNoneQuery("SP07_MM_MaterialInput_D1", CommandType.StoredProcedure,
            //                                        helper.CreateParameter("@PLANTCODE", Convert.ToString(row["PLANTCODE"])),
            //                                        helper.CreateParameter("@PONO"     , Convert.ToString(row["PONO"])));
            //                break;
            //            case DataRowState.Added:
            //                // 신규 발주 등록

            //                if (Convert.ToString(row["ITEMCODE"]) == "") sMessage += "아이템코드 ";
            //                if (Convert.ToString(row["POQTY"])    == "") sMessage += "발주수량 ";
            //                if (Convert.ToString(row["CUSTCODE"]) == "") sMessage += "거래처 ";
            //                if (sMessage != "")
            //                {
            //                    helper.Rollback();
            //                    ShowDialog(sMessage + "을(를) 입력해주세요");
            //                    return;
            //                }
            //                helper.ExecuteNoneQuery("SP07_MM_MaterialInput_I1", CommandType.StoredProcedure,
            //                                        helper.CreateParameter("@PLANTCODE", Convert.ToString(row["PLANTCODE"])),
            //                                        helper.CreateParameter("@ITEMCODE" , Convert.ToString(row["ITEMCODE"])),
            //                                        helper.CreateParameter("@PODATE"   , Convert.ToString(row["PODATE"])),
            //                                        helper.CreateParameter("@POQTY"    , Convert.ToString(row["POQTY"]).Replace(",","")),
            //                                        helper.CreateParameter("@UNITCODE" , Convert.ToString(row["UNITCODE"])),
            //                                        helper.CreateParameter("@CUSTCODE" , Convert.ToString(row["CUSTCODE"])),
            //                                        helper.CreateParameter("@MAKER"    , LoginInfo.UserID));

            //                break;
            //            case DataRowState.Modified:
            //                // 입고 등록을 하겠다! 라는 말
            //                helper.ExecuteNoneQuery("SP07_MM_MaterialInput_U1", CommandType.StoredProcedure,
            //                                        helper.CreateParameter("@PLANTCODE", Convert.ToString(row["PLANTCODE"])),
            //                                        helper.CreateParameter("@PONO"     , Convert.ToString(row["PONO"])),
            //                                        helper.CreateParameter("@INQTY"    , Convert.ToString(row["INQTY"]).Replace(",", "")),
            //                                        helper.CreateParameter("@INWORKER" , LoginInfo.UserID));

            //                break;
            //        }
            //        if(helper.RSCODE != "S")
            //        {
            //            helper.Rollback();
            //            ShowDialog($"등록중 오류가 발생하였습니다. \r\n {helper.RSMSG}");
            //            return;
            //        }
            //    }
            //    helper.Commit();
            //    ShowDialog("정상적으로 등록되었습니다.");
            //    DoInquire(); //등록된 데이터 재 조회.
            //}
            //catch (Exception ex)
            //{
            //    helper.Rollback();
            //    ShowDialog(ex.ToString());
            //}
            //finally
            //{
            //    helper.Close();
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            POP_MaterialInput hihihiihi = new POP_MaterialInput();
            hihihiihi.Show();
        }
    }
}
