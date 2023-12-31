USE [KDTB03_1JO]
GO
/****** Object:  StoredProcedure [dbo].[SP01_MM_MaterialOrder_S1]    Script Date: 2023-09-04 오후 6:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		강명서<Author,,Name>
-- Create date: 2023 8 14, 19 확정여부, 입고수량 추가
-- Description:	구매 자재 발주 조회<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SP01_MM_MaterialOrder_S1] -- 발주 조회
		
		@STARTDATE VARCHAR(10),  -- 발주 시작일자
		@ENDDATE   VARCHAR(10),  -- 발주 종료일자
		@PONO      VARCHAR(30),  -- 발주 번호
		@ITEMCODE  VARCHAR(30),  -- 품목코드
		@AORDERSTATUS VARCHAR(1), -- 자동발주여부
		@CONFIRM VARCHAR(1), -- 확정 여부

		@LANG      VARCHAR(10),
		@RS_CODE   VARCHAR(1)   OUTPUT,
		@RS_MSG    VARCHAR(100) OUTPUT
AS
BEGIN
	SELECT PLANTCODE											AS PLANTCODE, -- 공장
		   PONO													AS PONO,      -- 발주번호
		   ITEMCODE												AS ITEMCODE,  -- 품목코드
		   PODATE												AS PODATE,    -- 발주일자
		   POQTY												AS POQTY,     -- 발주수량
		   UNITCODE												AS UNITCODE,  -- 단위
		   CUSTCODE												AS CUSTCODE,  -- 거래처
		   (SELECT SUM (INPUTQTY) -- 입고 수량 합
			 FROM TB_MaterialInput A -- 입고 테이블을 A로 하자				
		    WHERE A.PONO      = B.PONO -- 발주 번호
			  AND A.PODATE    = B.PODATE -- 발주일
			  AND A.POSEQ     = B.POSEQ -- 발주 순번
			  AND A.PLANTCODE = B.PLANTCODE -- 공장
         GROUP BY PONO)											AS INPUTQTY,  -- 입고수량
		 -- 발주 번호 별로   
		 --AS INPUTQTY, -- 입고 수량
			 --발주의 키 정보랑 
			 --입고에 있는 발주쪽 연결고리 컬럼을( 포렌 키) 연결해주세요
			 --GROUP BY PONO
		   MAKEDATE												AS MAKEDATE,  -- 등록일시
		   DBO.FN_GET_USERNAME(MAKER)							AS MAKER, -- 등록자
		   

		   CASE ISNULL(AORDERSTATUS, 'N') WHEN 'N' THEN 'N'
										           ELSE 'Y' END AS AORDERSTATUS, -- 자동 발주 여부
		   CASE ISNULL(CONFIRM,      'N') WHEN 'N' THEN 'N' -- N 미확정
												   ELSE 'Y' END AS CONFIRM -- 확정 여부
														-- Y 확정
	  FROM TB_MaterialOrder B WITH(NOLOCK)
	 WHERE ITEMCODE  LIKE '%' + @ITEMCODE + '%' -- 품목코드
	   AND PONO      LIKE '%' + @PONO     + '%' -- 발주번호
	   AND PODATE    BETWEEN @STARTDATE AND @ENDDATE --발주일
	   AND CASE ISNULL(AORDERSTATUS, 'N') WHEN 'N' THEN 'N' -- 자동 발주 여부
										           ELSE AORDERSTATUS END LIKE '%' + @AORDERSTATUS + '%'
	   AND CASE ISNULL(CONFIRM,      'N') WHEN 'N' THEN 'N' -- 확정 여부
												   ELSE CONFIRM      END LIKE '%' + @CONFIRM      + '%'
	ORDER BY MAKEDATE
END		        
--SELECT * FROM TB_MaterialOrder
