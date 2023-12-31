USE [KDTB03_1JO]
GO
/****** Object:  StoredProcedure [dbo].[SP07_POP_MaterialInput_S1]    Script Date: 2023-09-04 오후 6:14:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		조해찬
-- Create date: 2023-08-14
-- Description:	입고 전 발주조회버튼 클릭 후 팝업창에서 발주조회 
-- =============================================
ALTER PROCEDURE [dbo].[SP07_POP_MaterialInput_S1]
	@PONO      VARCHAR(20),  -- 발주번호     
	@ITEMCODE  VARCHAR(20),  -- 품목
	@STARTDATE VARCHAR(20), 
	@ENDDATE   VARCHAR(20),

	@LANG      VARCHAR(10),  
	@RS_CODE   VARCHAR(1)   OUTPUT,
	@RS_MSG    VARCHAR(100) OUTPUT
	-- 발주LOT, 품목, 발주수량, 기발주수량, 입고가능수량
AS
BEGIN
	SELECT 0                           AS CHK 
	      ,A.PLANTCODE                 AS PLANTCODE
		  ,A.PONO                      AS PONO
	      ,A.ITEMCODE                  AS ITEMCODE
		  ,A.POQTY                     AS POQTY
		  ,ISNULL(B.POQTY,0)           AS ACCEPTQTY
		  ,0                           AS POSSIBLEQTY
		  ,A.PODATE                    AS PODATE
		  ,A.POSEQ                     AS POSEQ
	  FROM TB_MaterialOrder A WITH(NOLOCK)LEFT JOIN (SELECT PONO          AS PONO,
	                                                        SUM(INPUTQTY) AS POQTY
													   FROM TB_MaterialInput
												   GROUP BY PONO) B
												 ON A.PONO = B.PONO
	 WHERE A.POQTY<>ISNULL(B.POQTY,0)
	   AND A.ITEMCODE LIKE '%' + @ITEMCODE + '%'
	   AND A.PONO LIKE '%' + @PONO + '%'
	   AND A.PODATE BETWEEN @STARTDATE AND @ENDDATE
	ORDER BY PODATE
END
