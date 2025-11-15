// 이 인터페이스를 구현하는 모든 UI는 툴팁을 표시할 수 있는 기능을 갖게 됩니다.
// 툴팁에 표시될 제목과 내용을 제공해야 합니다.
public interface ITooltipProvider
{
    // 툴팁의 제목을 반환합니다.
    string GetTooltipTitle();

    // 툴팁의 상세 내용을 반환합니다.
    string GetTooltipContent();
}