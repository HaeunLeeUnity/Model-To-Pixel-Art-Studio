# Model To Pixel Art Studio
![image](https://github.com/user-attachments/assets/a65ee227-1d91-457f-b706-e3866032dec0)

## 개요
3D 모델을 도트 그래픽으로 변환하는 유니티 에디터 도구입니다.</br>
단일 장면 / 애니메이션을 픽셀화하여 도트 이미지로 저장합니다.</br>
Unity Job System 을 사용하여 작업 시간을 최적화했습니다.</br>
[Model To Pixel Art Studio 기술서](https://pleasant-cashew-a83.notion.site/Model-To-Pixel-Art-Studio-1878362504a780e1893fdec2ae534ebe)
</br>
</br>

## 사용 방법
프로젝트를 다운로드하여 유니티 에디터에서 실행합니다.</br>
StudioScene 에 모델을 배치하고 플레이 모드로 전환합니다.</br>
변환 또는 애니메이션 변환을 클릭합니다.</br>
![OneShot](https://github.com/user-attachments/assets/2bbd0f4c-b432-40a0-873f-d49cb55dd818)
![AnimationShot](https://github.com/user-attachments/assets/0c6ae456-f3dd-4009-8e46-c011d99858ae)


</br>

## 픽셀화 설정</br>
![image](https://github.com/user-attachments/assets/b2094f2d-6847-4d91-9866-c21a69aa42c7)

픽셀화 설정 버튼을 누르면 인스펙터에 현재 적용된 설정을 표시합니다.</br>
설정을 변경하여 결과물을 제어할 수 있습니다.</br>

![Pixelation Options](https://github.com/user-attachments/assets/ffd657bb-3854-413a-aba6-0dd83ac443b8)

</br>

## 설정 프리셋</br>
픽셀화 설정을 프리셋화 하여 저장하고 다시 사용할 수 있습니다.</br>

**신규 프리셋 생성**
1. 프로젝트 뷰 우클릭
2. Create - ModelToPixelArt - New Pixelation Option
![image](https://github.com/user-attachments/assets/bfa63672-9b74-4061-8095-5d331dfe1a0e)

</br>

**프리셋 변경**</br>
1. 하이어라키 뷰 ModelToPixelArtServiceController 클릭
2. 프리셋을 ModelToPixelArtServiceController - Pixelation Option 에 드래그
![image](https://github.com/user-attachments/assets/c6a51a82-5773-47f2-b01e-27ceda4f630a)



</br>

**사전 구성된 프리셋**</br>
[RESURRECT64](https://lospec.com/palette-list/resurrect-64) 
[ENDESGA32](https://lospec.com/palette-list/endesga-32) 
[LOSPEC](https://lospec.com/palette-list/lospec500) 
[MAME11](https://lospec.com/palette-list/mame11) 
[GRAPE7](https://lospec.com/palette-list/grape-7) 
[GOPURI16](https://lospec.com/palette-list/gopuri16)
