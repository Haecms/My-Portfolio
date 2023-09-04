using DC00_assm;
using DC00_Component;
using Infragistics.UltraChart.Core.Layers;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.AccessControl;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Telerik.Reporting;
/*************************************************************************************************************
 * 화면 ID  : MM_MatOrderApproval
 * 작성자   : 권문규
 * 작성일자 : 2023-08-08
 * 설명     : 현장발주내역 승인
 * ***********************************************************************************************************
 * 수정 이력 : 이름 변경 -> 구매 요청 관리
 * 수정 날짜 : 2023-08-12
 * 
 * 
 * ***********************************************************************************************************/

namespace KDTB_FORMS
{
    public partial class MM_MatOrderApproval : DC00_WinForm.BaseMDIChildForm
    {
        UltraGridUtil _GridUtil = new UltraGridUtil(); // 그리드 셋팅 클래스
        public MM_MatOrderApproval()
        {
            InitializeComponent();
        }

        private void MM_MatOrderApproval_Load(object sender, EventArgs e)
        {
            // 1. 그리드 셋팅
            
            _GridUtil.InitializeGrid(grid1);   // 싹 비우고 시작 Clear랑 같은 의미
            _GridUtil.InitColumnUltraGrid(grid1, "CHK"       , "승인"        , GridColDataType_emu.CheckBox    , 50 , HAlign.Left  , true, true);
            _GridUtil.InitColumnUltraGrid(grid1, "PLANTCODE" , "공장"        , GridColDataType_emu.VarChar     , 130, HAlign.Left  , true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "REQDATE"   , "요청일자"    , GridColDataType_emu.YearMonthDay, 130, HAlign.Center, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMCODE"  , "품목"        , GridColDataType_emu.VarChar     , 200, HAlign.Left  , true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "REQQTY"    , "발주요청수량", GridColDataType_emu.Double      , 100, HAlign.Right , true, true);
            _GridUtil.InitColumnUltraGrid(grid1, "UNITCODE"  , "단위"        , GridColDataType_emu.VarChar     , 100, HAlign.Center, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "CUSTNAME"  , "거래처"      , GridColDataType_emu.VarChar     , 100, HAlign.Left  , true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "APPRSTATUS", "승인여부"    , GridColDataType_emu.VarChar     , 70,  HAlign.Center, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "MAKER"     , "발주요청자"  , GridColDataType_emu.VarChar     , 130, HAlign.Center, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "MAKEDATE"  , "요청일시"    , GridColDataType_emu.DateTime24  , 130, HAlign.Left  , true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "EDITOR"    , "수정자"      , GridColDataType_emu.VarChar     , 130, HAlign.Center, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "EDITDATE"  , "수정일시"    , GridColDataType_emu.DateTime24  , 130, HAlign.Left  , true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "O/W"       , "발주장소"    , GridColDataType_emu.VarChar     , 80,  HAlign.Left  , false, false);
            _GridUtil.SetInitUltraGridBind(grid1);

            // 콤보박스 셋팅
            DataTable dtTemp = new DataTable();

            // 공장
            dtTemp = Common.StandardCODE("PLANTCODE");                    
            Common.FillComboboxMaster(cboPlantCode, dtTemp);              
            UltraGridUtil.SetComboUltraGrid(grid1, "PLANTCODE", dtTemp);

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
            

            // 승인 여부
            dtTemp = new DataTable();
            dtTemp.Columns.Add("CODE_ID");
            dtTemp.Columns.Add("CODE_NAME");
            dtTemp.Columns.Add("CODE_NAME_ORG");
            dtTemp.Rows.Add("Y", "[Y] 승인완료", "Y");
            dtTemp.Rows.Add("N", "[N] 승인대기중", "N");
            Common.FillComboboxMaster(cboApprstat, dtTemp);

            // 입출고 일자 기본 날짜 선택
            dtpStart.Value = string.Format("{0:yyyy-MM-01}", DateTime.Now);
            dtpEnd.Value   = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
        }

        public override void DoInquire()
        {
            // 구매자재 발주 및 입고 내역 조회
            string sPlantCode  = Convert.ToString(cboPlantCode.Value);            // 공장
            string sCustCode   = Convert.ToString(cboCustCode.Value);             // 거래처
            string sItemCode   = Convert.ToString(cboItemCode.Value);             // 품목코드
            string sStartDate  = string.Format("{0:yyyy-MM-dd}", dtpStart.Value); // 발주요청 시작일자
            string sEndDate    = string.Format("{0:yyyy-MM-dd}", dtpEnd.Value);   // 발주요청 종료일자
            string sApprstat   = Convert.ToString(cboApprstat.Value);

            DBHelper helper = new DBHelper(false);
            try
            {
                _GridUtil.Grid_Clear(grid1);    // 그리드 데이터 행 삭제 
                // Database에서 작업자 정보 조회
                DataTable dtTemp = new DataTable();
                dtTemp = helper.FillTable("_1JO_MM_MatOrderApproval_S1", CommandType.StoredProcedure
                                         , helper.CreateParameter("@PLANTCODE", sPlantCode)
                                         , helper.CreateParameter("@CUSTCODE" , sCustCode)
                                         , helper.CreateParameter("@ITEMCODE" , sItemCode)
                                         , helper.CreateParameter("@STARTDATE", sStartDate)
                                         , helper.CreateParameter("@ENDDATE"  , sEndDate)
                                         , helper.CreateParameter("@APPRSTAT" , sApprstat));
                grid1.DataSource = dtTemp;
                
                
                if (grid1.Rows.Count == 0)
                {
                    ClosePrgForm(); //프로그레스 상태 창 닫기
                    ShowDialog("조회할 데이터가 없습니다.");
                }
                
                // 이거 하고싶은데 안되네요
                //foreach (UltraGridRow row in grid1.Rows)
                //{
                //    if (Convert.ToString(row.Cells[7]) == "Y")
                //    {
                //        row.Cells["CHK"].Value = 1;
                //        row.Cells["CHK"].Activation = Activation.NoEdit;
                //    }
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

        public override void DoSave()
        {
            // 변경된 이력이 있는 행을 데이터 테이블에 담기
            DataTable dt = grid1.chkChange();
            Queue qMessage = new Queue();
            string sMessage = "";
            if (dt == null)
            {
                ShowDialog("변경 사항이 없습니다. 확인 후 다시 시도해주세요.");
                return;
            }
            if (dt.Rows.Count == 0)
            {
                ShowDialog("변경 사항이 없습니다. 확인 후 다시 시도해주세요.");
                return;
            }

            // 확인 메시지
            if (ShowDialog("저장하시겠습니까?") == DialogResult.Cancel)
            {
                return;
            }

            DBHelper helper = new DBHelper(true);
            string sPlantcode = string.Empty;
            string sItemcode  = string.Empty;
            string sUnitcode  = string.Empty;
            string sCustname  = string.Empty;
            string sMaker     = string.Empty;
            string sOW        = string.Empty;
            int    iPOQTY;
            
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    // row.RejectChanges();
                    DateTime sDate = new DateTime();
                    if (row.RowState != DataRowState.Deleted)
                    {
                        sPlantcode = Convert.ToString(row["PLANTCODE"]);
                        sMaker = Convert.ToString(LoginInfo.UserID);
                    }
                    
                    

                    switch (row.RowState)
                    {
                        case DataRowState.Unchanged:
                            break;

                        case DataRowState.Deleted:
                            row.RejectChanges();
                            sDate = Convert.ToDateTime(row["MAKEDATE"]);
                            helper.ExecuteNoneQuery("_1JO_MM_MatOrderApproval_D1", CommandType.StoredProcedure
                                                  , helper.CreateParameter("@DATE", sDate));
                            break;

                        case DataRowState.Modified:
                            // 체크박스가 변경되었는지, 수량이 변경되었는지 구분해야하는가?
                            // 체크도 하고 수량도 변경했으면 어떡할거야
                            // 발주승인품목은 Modified 할 수 없도록 해야함.

                            sItemcode = Convert.ToString(row["ITEMCODE"]);
                            iPOQTY    = Convert.ToInt32(row["REQQTY"]);
                            sUnitcode = Convert.ToString(row["UNITCODE"]);
                            sCustname = Convert.ToString(row["CUSTNAME"]);
                            sDate     = Convert.ToDateTime(row["MAKEDATE"]);
                            sOW       = Convert.ToString(row["O/W"]);
                            if (Convert.ToInt32(row["CHK"]) == 0)
                            {
                                helper.ExecuteNoneQuery("_1JO_MM_MatOrderApproval_U1", CommandType.StoredProcedure
                                                       , helper.CreateParameter("@DATE", sDate)
                                                       , helper.CreateParameter("@REQQTY", iPOQTY)
                                                       , helper.CreateParameter("@EDITOR", sMaker));
                            }
                            else
                            {
                                // 체크가 1인 경우니까, TB_OrderRequestList에도 Update 해주고, TB_MaterialOrder에도 Insert 해줘야 함.
                                helper.ExecuteNoneQuery("_1JO_MM_MatOrderApproval_U2", CommandType.StoredProcedure
                                                   , helper.CreateParameter("@PLANTCODE", sPlantcode)
                                                   , helper.CreateParameter("@ITEMCODE" , sItemcode)
                                                   , helper.CreateParameter("@POQTY"    , iPOQTY)
                                                   , helper.CreateParameter("@UNITCODE" , sUnitcode)
                                                   , helper.CreateParameter("@CUSTNAME" , sCustname)
                                                   , helper.CreateParameter("@DATE"     , sDate)
                                                   , helper.CreateParameter("@MAKER"    , sMaker)
                                                   , helper.CreateParameter("@OW"       , sOW));
                            }
                            if (helper.RSCODE != "S")
                            {
                                row.RejectChanges();
                            }
                            
                            break;

                        case DataRowState.Added:
                            if (Convert.ToString(row["ITEMCODE"]) == "") qMessage.Enqueue("품목");
                            if (Convert.ToString(row["REQQTY"]) == "") qMessage.Enqueue("수량");
                            while (qMessage.Count > 0)
                            {
                                if (sMessage == "")
                                    sMessage += qMessage.Dequeue();
                                else
                                    sMessage += ", " + qMessage.Dequeue();
                            }
                            if (sMessage != "")
                            {
                                helper.Rollback();
                                ShowDialog(sMessage + "를 입력해주세요.");
                                return;
                            }
                            row["O/W"] = "O";

                            string sReqdate = Convert.ToString(row["REQDATE"]);
                            string sCHK = Convert.ToString(row["CHK"]);
                            sItemcode = Convert.ToString(row["ITEMCODE"]);
                            iPOQTY    = Convert.ToInt32(row["REQQTY"]);
                            sUnitcode = Convert.ToString(row["UNITCODE"]);
                            sCustname = Convert.ToString(row["CUSTNAME"]);
                            sOW       = Convert.ToString(row["O/W"]);

                            helper.ExecuteNoneQuery("_1JO_MM_MatOrderApproval_I1", CommandType.StoredProcedure
                                                   , helper.CreateParameter("@PLANTCODE", sPlantcode)
                                                   , helper.CreateParameter("@REQDATE"  , sReqdate)
                                                   , helper.CreateParameter("@ITEMCODE" , sItemcode)
                                                   , helper.CreateParameter("@REQQTY"   , iPOQTY)
                                                   , helper.CreateParameter("@UNITCODE" , sUnitcode)
                                                   , helper.CreateParameter("@CUSTNAME" , sCustname)
                                                   , helper.CreateParameter("@CHK"      , sCHK)
                                                   , helper.CreateParameter("@MAKER"    , sMaker)
                                                   , helper.CreateParameter("@OW"       , sOW));
                            break;
                    }

                    if (helper.RSCODE != "S")
                    {
                        helper.Rollback();
                        ShowDialog("등록 중 오류 발생! \r\n" + helper.RSMSG);
                        return;
                    }
                }
                helper.Commit();
                
                ShowDialog("정상적으로 저장되었습니다.");
                DoInquire();
            }
            catch (Exception ex)
            {
                helper.Rollback();
                ShowDialog($"등록중 오류가 발생하였습니다.\r\n{ex.Message}");
                return;
            }
            finally
            {
                helper.Close();
            }
        }

        public override void DoNew()
        {
            grid1.InsertRow();

            grid1.ActiveRow.Cells["PLANTCODE"].Value = LoginInfo.PlantCode;                 // 공장 : 로그인 할 때 당시의 플랜트코드값이 셋팅됨
            grid1.ActiveRow.Cells["REQDATE"].Value   = DateTime.Now.ToString("yyyy-MM-dd");
            grid1.ActiveRow.Cells["MAKER"].Value     = LoginInfo.UserID;
            grid1.ActiveRow.Cells["CHK"].Value       = 0;

            grid1.ActiveRow.Cells["PLANTCODE"].Activation  = Activation.NoEdit;
            grid1.ActiveRow.Cells["REQDATE"].Activation    = Activation.NoEdit;
            grid1.ActiveRow.Cells["UNITCODE"].Activation   = Activation.NoEdit;
            grid1.ActiveRow.Cells["CUSTNAME"].Activation   = Activation.NoEdit;
            grid1.ActiveRow.Cells["APPRSTATUS"].Activation = Activation.NoEdit;
            grid1.ActiveRow.Cells["MAKEDATE"].Activation   = Activation.NoEdit;
            grid1.ActiveRow.Cells["MAKER"].Activation      = Activation.NoEdit;
            grid1.ActiveRow.Cells["EDITOR"].Activation     = Activation.NoEdit;
            grid1.ActiveRow.Cells["EDITDATE"].Activation   = Activation.NoEdit;
        }

        public override void DoDelete()
        {
            grid1.DeleteRow();
        }

        private void grid1_CellListSelect(object sender, CellEventArgs e)
        {
            if (null != e.Cell.Column.ValueList)
            {
                // e.Cell.Text ==> "[KDTB02-ROH] 베어링 볼"을 가공해서 ITEMCODE만 뽑아오게
                string sCellValue = e.Cell.Text;
                int iSindex = sCellValue.IndexOf('[');
                int iFindex = sCellValue.IndexOf(']');
                string sItemcode = sCellValue.Substring(iSindex + 1, iFindex - iSindex - 1);

                DBHelper helper = new DBHelper();
                try
                {
                    DataTable dtTemp = new DataTable();
                    dtTemp = helper.FillTable("_1JO_MM_MatOrderApproval_S2", CommandType.StoredProcedure
                                             , helper.CreateParameter("@ITEMCODE", sItemcode));
                    if (dtTemp.Rows.Count > 0)
                    {
                        grid1.ActiveRow.Cells["UNITCODE"].Value = Convert.ToString(dtTemp.Rows[0]["EA"]);
                        grid1.ActiveRow.Cells["CUSTNAME"].Value = Convert.ToString(dtTemp.Rows[0]["CUSTNAME"]);
                    }
                    else
                    {
                        grid1.ActiveRow.Cells["UNITCODE"].Value = "";
                        grid1.ActiveRow.Cells["CUSTNAME"].Value = "";
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
        }

        private void grid1_AfterRowActivate(object sender, EventArgs e)
        {
            if (Convert.ToString(grid1.ActiveRow.Cells["CHK"]) == "1" && grid1.ActiveRow.Cells["APPRSTATUS"].Text == "Y")
                    grid1.ActiveRow.Cells["CHK"].Activation = Activation.NoEdit;
        }
    }
}
