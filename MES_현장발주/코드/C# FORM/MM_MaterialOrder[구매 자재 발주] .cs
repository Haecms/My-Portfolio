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
 * 화면 ID  : MM_MaterialOrder
 * 작성자   : 조해찬
 * 작성일자 : 2023-05-17
 * 설명     : 구매 자재 발주 등록 및 입고 등록 (관리)
 * ***********************************************************************************************************
 * 수정 이력 : 강명서 2023 8 14 월 구매 자재 발주 화면 만들어보기 - 해찬 샘 과제
 * 2023 8 14 월 - 조회 가능
 * 2023 8 15 화 - 행 선택하고 삭제 버튼 클릭 후 저장도 가능
 * 2023 8 16 수 - 추가 버튼 클릭 후 저장도 가능
 * 2023 8 1
 * 
 * ***********************************************************************************************************/

namespace KDTB_FORMS
{
    public partial class MM_MaterialOrder : DC00_WinForm.BaseMDIChildForm
    {
        UltraGridUtil _GridUtil = new UltraGridUtil(); // 그리드 셋팅 클래스
        public MM_MaterialOrder()
        {
            InitializeComponent();
        }

        private void MM_MaterialOrder_Load(object sender, EventArgs e)
        {
            // 1. 그리드 셋팅
            
            _GridUtil.InitializeGrid(grid1);   // 싹 비우고 시작 Clear랑 같은 의미
            _GridUtil.InitColumnUltraGrid(grid1, "PLANTCODE", "공장"    , GridColDataType_emu.VarChar        , 130, HAlign.Left , true, true);
            _GridUtil.InitColumnUltraGrid(grid1, "PONO"     , "발주번호", GridColDataType_emu.VarChar        , 130, HAlign.Left , true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMCODE" , "품목"    , GridColDataType_emu.VarChar        , 200, HAlign.Left , true, true);
            _GridUtil.InitColumnUltraGrid(grid1, "PODATE"   , "발주일자", GridColDataType_emu.YearMonthDay   , 100, HAlign.Left , true, true);
            _GridUtil.InitColumnUltraGrid(grid1, "POQTY"    , "발주수량", GridColDataType_emu.Double         , 100, HAlign.Right, true, true);
            _GridUtil.InitColumnUltraGrid(grid1, "INPUTQTY", "입고수량", GridColDataType_emu.Integer,          130, HAlign.Right, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "UNITCODE" , "단위"    , GridColDataType_emu.VarChar        , 100, HAlign.Left , true, true);
            _GridUtil.InitColumnUltraGrid(grid1, "CUSTCODE" , "거래처"  , GridColDataType_emu.VarChar        , 100, HAlign.Left , true, true);

            _GridUtil.InitColumnUltraGrid(grid1, "AORDERSTATUS", "자동발주여부", GridColDataType_emu.VarChar, 100, HAlign.Center, true, false);
            //_GridUtil.InitColumnUltraGrid(grid1, "CONFIRM", "확정여부", GridColDataType_emu.VarChar, 100, HAlign.Center, true, false);
            //_GridUtil.InitColumnUltraGrid(grid1, "CHK"      , "입고등록", GridColDataType_emu.CheckBox       , 80 , HAlign.Left , true, true);
            //_GridUtil.InitColumnUltraGrid(grid1, "INQTY"    , "입고수량", GridColDataType_emu.Double         , 100, HAlign.Right, true, true);
            //                                                                                                 
            //_GridUtil.InitColumnUltraGrid(grid1, "LOTNO"    , "LOT번호" , GridColDataType_emu.VarChar        , 150, HAlign.Left , true, false);
            //_GridUtil.InitColumnUltraGrid(grid1, "INDATE"   , "입고일자", GridColDataType_emu.VarChar        , 100, HAlign.Left , true, false);
            //_GridUtil.InitColumnUltraGrid(grid1, "INWORKER" , "입고자"  , GridColDataType_emu.VarChar        , 150, HAlign.Left , true, false);

            // 발주 일자 + 시분초
            _GridUtil.InitColumnUltraGrid(grid1, "MAKEDATE" , "등록일시", GridColDataType_emu.DateTime24     , 130, HAlign.Center, true, false);
            
            _GridUtil.InitColumnUltraGrid(grid1, "MAKER"    , "등록자"  , GridColDataType_emu.VarChar        , 130, HAlign.Left  , true, false);
            //_GridUtil.InitColumnUltraGrid(grid1, "EDITDATE" , "수정일시", GridColDataType_emu.DateTime24     , 130, HAlign.Center, true, false);
            //_GridUtil.InitColumnUltraGrid(grid1, "EDITOR"   , "수정자"  , GridColDataType_emu.VarChar        , 130, HAlign.Left  , true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "CONFIRM", "확정여부", GridColDataType_emu.VarChar, 100, HAlign.Center, true, false);
            _GridUtil.SetInitUltraGridBind(grid1); // 디스플레이랑 ?을 실시간으로 바인딩

            // 콤보박스 셋팅
            DataTable dtTemp = new DataTable();

            // 공장
            dtTemp = Common.StandardCODE("PLANTCODE");                    
            Common.FillComboboxMaster(cboPlantCode, dtTemp);              
            UltraGridUtil.SetComboUltraGrid(grid1, "PLANTCODE", dtTemp);

            // 자동 발주 여부
            dtTemp = Common.StandardCODE("AORDERSTATUS");
            Common.FillComboboxMaster(cboAOrderFlag, dtTemp);

            // 확정 여부
            dtTemp = Common.StandardCODE("AORDERSTATUS");
            Common.FillComboboxMaster(cboConfirm, dtTemp);
           // UltraGridUtil.SetComboUltraGrid(grid1, "CONFIRM", dtTemp);

            // 협력업체 (거래처)
            dtTemp = Common.GET_Cust_Code("V");                           
            Common.FillComboboxMaster(cboCustCode, dtTemp);               
            UltraGridUtil.SetComboUltraGrid(grid1, "CUSTCODE", dtTemp);   

            // 거래처
            dtTemp = Common.StandardCODE("UNITCODE");                     
            UltraGridUtil.SetComboUltraGrid(grid1, "UNITCODE", dtTemp);   

            // 발주 품목
            dtTemp = Common.Get_ItemCode(new string[] {"ROH"});           
            Common.FillComboboxMaster(cboItemCode, dtTemp);
            UltraGridUtil.SetComboUltraGrid(grid1, "ITEMCODE", dtTemp);

            // 입출고 일자 기본 날짜 선택
            dtpStart.Value = string.Format("{0:yyyy-MM-01}", DateTime.Now);
            dtpEnd.Value   = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
        }

        public override void DoInquire() // 조회
        {
            // 구매자재 발주 내역 조회
            //string sPlantCode  = Convert.ToString(cboPlantCode.Value);            // 공장
            //string sCustCode   = Convert.ToString(cboCustCode.Value);             // 사번
            string sStartDate  = string.Format("{0:yyyy-MM-dd}", dtpStart.Value); // 등록 시작일자
            string sEndDate    = string.Format("{0:yyyy-MM-dd}", dtpEnd.Value);   // 등록 종료일자
            string sPono       = Convert.ToString(txtPono.Text);                  // 발주번호
            string sItemCode   = Convert.ToString(cboItemCode.Value);             // 품목 코드
            string sAorderFlag = Convert.ToString(cboAOrderFlag.Value); // 자동 발주 여부
            string sConfirm = Convert.ToString(cboConfirm.Value); // 확정 여부

            DBHelper helper = new DBHelper(false);
            try
            {
                _GridUtil.Grid_Clear(grid1);    // 그리드 데이터 행 삭제 
                // Database에서 작업자 정보 조회
                DataTable dtTemp = new DataTable();
                dtTemp = helper.FillTable("SP01_MM_MaterialOrder_S1", CommandType.StoredProcedure // 조회
                                         //, helper.CreateParameter("@PLANTCODE", sPlantCode) // 공장
                                         //, helper.CreateParameter("@CUSTCODE" , sCustCode) // 거래처 코드
                                         , helper.CreateParameter("@STARTDATE", sStartDate) // 등록 시작일
                                         , helper.CreateParameter("@ENDDATE"  , sEndDate) // 등록 종료일
                                         , helper.CreateParameter("@PONO"     , sPono) // 발주 번호
                                         , helper.CreateParameter("@ITEMCODE" , sItemCode) // 품목 코드
                                         , helper.CreateParameter("@CONFIRM", sConfirm) // 확정 여부

                                         , helper.CreateParameter("@AORDERSTATUS", sAorderFlag)); // 자동 발주 여부
                grid1.DataSource = dtTemp;
                if (grid1.Rows.Count == 0)
                {
                    ClosePrgForm(); //프로그레스 상태 창 닫기
                    ShowDialog("조회 할 데이터가 없습니다.");
                }
                for ( int i =0; i <  grid1.Rows.Count; i++)
                {
                    grid1.Rows[i].Cells["PONO"].Activation     = Activation.NoEdit; // 발주 번호
                    grid1.Rows[i].Cells["MAKEDATE"].Activation = Activation.NoEdit; // 등록일
                    grid1.Rows[i].Cells["MAKER"].Activation    = Activation.NoEdit; // 등록자
                    //grid1.Rows[i].Cells["EDITDATE"].Activation = Activation.NoEdit; // 수정일
                    //grid1.Rows[i].Cells["EDITOR"].Activation   = Activation.NoEdit; // 수정자
                } 

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
        public override void DoNew() // 추가
        {
            // 새로운 행 생성 (구매자재 발주 등록을 위한 추가)
            grid1.InsertRow();

            // 기본 값 셋팅
            grid1.ActiveRow.Cells["PLANTCODE"].Value = LoginInfo.PlantCode;                 // 공장 : 로그인 할 때 당시의 플랜트코드값이 셋팅됨
            grid1.ActiveRow.Cells["PODATE"].Value    = DateTime.Now.ToString("yyyy-MM-dd"); // 발주 일자                               // 그룹

            // 수정 불가 컬럼 셋팅
            grid1.ActiveRow.Cells["PONO"].Activation     = Activation.NoEdit; // 발주 번호
            grid1.ActiveRow.Cells["MAKEDATE"].Activation = Activation.NoEdit; // 등록일
            grid1.ActiveRow.Cells["MAKER"].Activation    = Activation.NoEdit; // 등록자
            //grid1.ActiveRow.Cells["EDITDATE"].Activation = Activation.NoEdit; // 수정일
            //grid1.ActiveRow.Cells["EDITOR"].Activation   = Activation.NoEdit; // 수정자
        }

        public override void DoDelete() // 삭제
        {
            grid1.DeleteRow();
        }

        public override void DoSave() // 저장
        {
            // 1. 변경된 이력이 있는 행을 데이터 테이블에 담기
            DataTable dt = grid1.chkChange();
            if (dt.Rows.Count == 0) return;

            // 저장하시겠습니까?
            if (ShowDialog("변경된 데이터를 저장 하시겠습니까?") == DialogResult.Cancel)
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
                    switch (row.RowState)
                    {
                        case DataRowState.Deleted:
                            // 발주 취소
                            row.RejectChanges();
                            helper.ExecuteNoneQuery("SP07_MM_MaterialOrderIn_D1", CommandType.StoredProcedure, // 삭제
                                                     helper.CreateParameter("@PLANTCODE", Convert.ToString(row["PLANTCODE"])), // 공장
                                                     helper.CreateParameter("@PONO"     , Convert.ToString(row["PONO"]))); // 발주 번호
                            break;
                        case DataRowState.Added:
                            // 신규 발주 등록

                            if (Convert.ToString(row["ITEMCODE"]) == "") sMessage += "아이템코드 ";
                            if (Convert.ToString(row["POQTY"])    == "") sMessage += "발주수량 ";
                            if (Convert.ToString(row["CUSTCODE"]) == "") sMessage += "거래처 ";
                            if (Convert.ToString(row["UNITCODE"]) == "") sMessage += "단위 ";
                            if (sMessage != "")
                            {
                                helper.Rollback();
                                ShowDialog(sMessage + "을(를) 입력해주세요");
                                return;
                            }
                            helper.ExecuteNoneQuery("SP01_MM_MaterialOrder_I1", CommandType.StoredProcedure, // 발주 등록
                                                    helper.CreateParameter("@PLANTCODE", Convert.ToString(row["PLANTCODE"])), // 공장
                                                    helper.CreateParameter("@ITEMCODE" , Convert.ToString(row["ITEMCODE"])), // 품목
                                                    helper.CreateParameter("@PODATE"   , Convert.ToString(row["PODATE"])), // 발주일
                                                    helper.CreateParameter("@POQTY"    , Convert.ToString(row["POQTY"]).Replace(",","")), // 발주수량
                                                    helper.CreateParameter("@UNITCODE" , Convert.ToString(row["UNITCODE"])), // 단위
                                                    helper.CreateParameter("@CUSTCODE" , Convert.ToString(row["CUSTCODE"])), // 거래처
                                                    helper.CreateParameter("@MAKEDATE", Convert.ToString(row["MAKEDATE"])), // 등록일
                                                    helper.CreateParameter("@MAKER"    , LoginInfo.UserID)); // 등록자
                            // 8개
                            break;
                        case DataRowState.Modified:
                            // 발주 등록을 하겠다! 라는 말
                            helper.ExecuteNoneQuery("SP01_MM_MaterialOrder_U1", CommandType.StoredProcedure, // 발주 내역으로 등록
                                                    helper.CreateParameter("@PLANTCODE", Convert.ToString(row["PLANTCODE"])), // 공장
                                                    helper.CreateParameter("@PONO", Convert.ToString(row["PONO"])), // 발주 번호
                                                    helper.CreateParameter("@POQTY", Convert.ToString(row["POQTY"])), // 발주 수량
                                                    helper.CreateParameter("@MAKEDATE", Convert.ToString(row["MAKER"])), // 등록일 
                                                    helper.CreateParameter("@ITEMCODE", Convert.ToString(row["ITEMCODE"])), // 품목코드
                                                    helper.CreateParameter("@MAKER", LoginInfo.UserID)); // 등록자
                                                    
                                                    //helper.CreateParameter("@ITEMCODE", Convert.ToString(row["ITEMCODE"]))); // 품목코드
                                                    //helper.CreateParameter("@ITEMCODE", Convert.ToString(row["ITEMCODE"]));
                                                    //helper.CreateParameter("@INQTY", Convert.ToString(row["INQTY"]).Replace(",", "")), // 입고수량
                                                    //helper.CreateParameter("@INWORKER", LoginInfo.UserID)); // 입고자

                            break;
                    }
                    if(helper.RSCODE != "S")
                    {
                        helper.Rollback();
                        ShowDialog($"등록중 오류가 발생하였습니다. \r\n {helper.RSMSG}");
                        return;
                    }
                }
                helper.Commit();
                ShowDialog("정상적으로 등록되었습니다.");
                DoInquire(); //등록된 데이터 재 조회.
            }
            catch (Exception ex)
            {
                helper.Rollback();
                ShowDialog(ex.ToString());
            }
            finally
            {
                helper.Close();
            }
        }

        private void sLabel3_Click(object sender, EventArgs e)
        {

        }


        private void txtPono_ValueChanged(object sender, EventArgs e)
        {

        }
    }
} // 2023 8 15 - 조회, 삭제만 가능
