USE [KDTB03_1JO]
GO
/****** Object:  StoredProcedure [dbo].[_1JO_MM_MatOrderApproval_S2]    Script Date: 2023-09-04 오후 6:25:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		권문규
-- Create date: 2023-08-12
-- Description:	구매 요청 관리 창에서 새로운 행 추가해서 데이터 입력할 때, 품목코드 넣으면 단위랑 거래처 받아오는 프로시저
-- =============================================
ALTER PROCEDURE [dbo].[_1JO_MM_MatOrderApproval_S2]
	@ITEMCODE VARCHAR(30),   -- 품목코드

	@LANG VARCHAR(10),
	@RS_CODE VARCHAR(1)   OUTPUT,
	@RS_MSG  VARCHAR(100) OUTPUT
AS
BEGIN
	SELECT ISNULL(A.BASEUNIT, '')    AS EA            -- 단위
	      ,ISNULL(B.CUSTNAME, '')    AS CUSTNAME      -- 거래처
		  ,ISNULL(A.ORDERQTY, 0)     AS REQQTY        -- 자동발주수량
	  FROM TB_ItemMaster A WITH(NOLOCK) JOIN TB_CustMaster B WITH(NOLOCK)
	                                      ON A.MAKECOMPANY = B.CUSTCODE
	 WHERE ITEMCODE = @ITEMCODE
END
SELECT *
  FROM TB_ItemMaster