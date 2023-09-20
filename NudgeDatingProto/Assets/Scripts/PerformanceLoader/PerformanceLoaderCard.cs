using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Shapes;

public class PerformanceLoaderCard : MonoBehaviour
{
    [Header("Animation Controllers")]
    [SerializeField] private ShapesOpacityAnimation outlineOpacity;
    [Space(5)]
    [SerializeField] private MoveAnimation titleMove;
    [SerializeField] private TmpOpacityAnimation titleOpacity;
    [Space(5)]
    [SerializeField] private MoveAnimation contentMove;
    [SerializeField] private TmpOpacityAnimation contentOpacity;
    [Space(5)]
    [SerializeField] private MoveAnimation hMarkerMove;
    [SerializeField] private TmpOpacityAnimation hMarkerTextOpacity;
    [SerializeField] private ImageOpacityAnimation hMarkerFillOpacity;
    [SerializeField] private ImageOpacityAnimation hMarkerOutlineOpacity;
    [Space(5)]
    [SerializeField] private MoveAnimation nMarkerMove;
    [SerializeField] private TmpOpacityAnimation nMarkerTextOpacity;
    [SerializeField] private ShapesOpacityAnimation nMarkerOutlineOpacity;
    [Space(5)]
    [SerializeField] private MoveAnimation cMarkerMove;
    [SerializeField] private ShapesOpacityAnimation cMarkerCheckOpacity;
    [SerializeField] private ShapesOpacityAnimation cMarkerFillOpacity;
    [SerializeField] private ShapesOpacityAnimation cMarkerOutlineOpacity;
    [Space(5)]
    [SerializeField] private Button okButton;
    [SerializeField] private ChangeCursorOnHover buttonHover;
    [SerializeField] private BoxCollider2D buttonHoverCollider;
    [SerializeField] private MoveAnimation okButtonMove;
    [SerializeField] private TmpOpacityAnimation okButtonTextOpacity;
    [SerializeField] private ShapesOpacityAnimation okButtonOutlineOpacity;
    [SerializeField] private ShapesOpacityAnimation okButtonFillOpacity;
    [SerializeField] private ShapesOpacityAnimation okButtonFillBgOpacity;
    [SerializeField] private ShapesOpacityAnimation okButtonShadowOpacity;

    public void AnimateToHighlighted(float time)
    {
        outlineOpacity.AnimTime = time/2f;
        outlineOpacity.AnimateOpacityTo(1f);

        titleMove.MoveTime = time;
        titleMove.TargetLocationY = -65f;
        titleMove.MoveTo();
        titleOpacity.AnimTime = time;
        titleOpacity.AnimateOpacityTo(1f);

        contentMove.MoveTime = time;
        contentMove.TargetLocationY = -135f;
        contentMove.MoveTo();
        contentOpacity.AnimTime = time;
        contentOpacity.AnimateOpacityTo(1f);

        hMarkerMove.MoveTime = time;
        hMarkerMove.TargetLocationY = 5f;
        hMarkerMove.MoveTo();
        hMarkerTextOpacity.AnimTime = time/2f;
        hMarkerTextOpacity.AnimateOpacityTo(1f);
        hMarkerFillOpacity.AnimTime = time/2f;
        hMarkerFillOpacity.AnimateOpacityTo(1f);
        hMarkerOutlineOpacity.AnimTime = time/2f;
        hMarkerOutlineOpacity.AnimateOpacityTo(1f);

        nMarkerMove.MoveTime = time;
        nMarkerMove.TargetLocationY = 5f;
        nMarkerMove.MoveTo();
        nMarkerTextOpacity.AnimTime = time;
        nMarkerTextOpacity.AnimateOpacityTo(0f);
        nMarkerOutlineOpacity.AnimTime = time;
        nMarkerOutlineOpacity.AnimateOpacityTo(0f);

        okButton.enabled = true;
        buttonHover.enabled = true;
        buttonHoverCollider.enabled = true;
        okButtonTextOpacity.AnimTime = time / 2f;
        okButtonTextOpacity.AnimateOpacityTo(1f);
        okButtonOutlineOpacity.AnimTime = time / 2f;
        okButtonOutlineOpacity.AnimateOpacityTo(1f);
        okButtonFillOpacity.AnimTime = time / 2f;
        okButtonFillOpacity.AnimateOpacityTo(0.4f);
        okButtonFillBgOpacity.AnimTime = time / 2f;
        okButtonFillBgOpacity.AnimateOpacityTo(1f);
        okButtonShadowOpacity.AnimTime = time / 2f;
        okButtonShadowOpacity.AnimateOpacityTo(1f);
    }

    public void AnimateToDone(float time)
    {
        outlineOpacity.AnimTime = time/2f;
        outlineOpacity.AnimateOpacityTo(0f);

        titleMove.MoveTime = time;
        titleMove.TargetLocationY = -40f;
        titleMove.MoveTo();
        titleOpacity.AnimTime = time;
        titleOpacity.AnimateOpacityTo(0.4f);

        contentMove.MoveTime = time;
        contentMove.TargetLocationY = -110f;
        contentMove.MoveTo();
        contentOpacity.AnimTime = time;
        contentOpacity.AnimateOpacityTo(0.4f);

        hMarkerMove.MoveTime = time;
        hMarkerMove.TargetLocationY = 0f;
        hMarkerMove.MoveTo();
        hMarkerTextOpacity.AnimTime = time/2f;
        hMarkerTextOpacity.AnimateOpacityTo(0f);
        hMarkerFillOpacity.AnimTime = time/2f;
        hMarkerFillOpacity.AnimateOpacityTo(0f);
        hMarkerOutlineOpacity.AnimTime = time/2f;
        hMarkerOutlineOpacity.AnimateOpacityTo(0f);

        nMarkerMove.MoveTime = time;
        nMarkerMove.TargetLocationY = 0f;
        nMarkerMove.MoveTo();
        nMarkerTextOpacity.AnimTime = time;
        nMarkerTextOpacity.AnimateOpacityTo(0.4f);
        nMarkerOutlineOpacity.AnimTime = time;
        nMarkerOutlineOpacity.AnimateOpacityTo(0.4f);

        okButton.enabled = false;
        buttonHover.enabled = false;
        buttonHoverCollider.enabled = false;
        okButtonMove.MoveTime = time;
        okButtonMove.TargetLocationY = -17f;
        okButtonMove.MoveTo();
        okButtonTextOpacity.AnimTime = time/2f;
        okButtonTextOpacity.AnimateOpacityTo(0f);
        okButtonOutlineOpacity.AnimTime = time/2f;
        okButtonOutlineOpacity.AnimateOpacityTo(0f);
        okButtonFillOpacity.AnimTime = time/2f;
        okButtonFillOpacity.AnimateOpacityTo(0f);
        okButtonFillBgOpacity.AnimTime = time/2f;
        okButtonFillBgOpacity.AnimateOpacityTo(0f);
        okButtonShadowOpacity.AnimTime = time/2f;
        okButtonShadowOpacity.AnimateOpacityTo(0f);

        cMarkerMove.MoveTime = time;
        cMarkerMove.TargetLocationY = -17.5f;
        cMarkerMove.MoveTo();
        cMarkerCheckOpacity.AnimTime = time;
        cMarkerCheckOpacity.AnimateOpacityTo(0.4f);
        cMarkerFillOpacity.AnimTime = time;
        cMarkerFillOpacity.AnimateOpacityTo(0.4f);
        cMarkerOutlineOpacity.AnimTime = time;
        cMarkerOutlineOpacity.AnimateOpacityTo(0.4f);
    }
}
