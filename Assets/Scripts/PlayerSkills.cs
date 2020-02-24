using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionSkills {None, SkillOne, SkillTwo,SkillThree,SkillFour, SkillFive }
public enum PassiveSkills { None, SkillOne, SkillTwo, SkillThree, SkillFour, SkillFive }
public class PlayerSkills : MonoBehaviour
{
    public ActionSkills actionSkill;
    public PassiveSkills passiveSkills;

    public void ActionButtonPress ()
    {
        switch (actionSkill)
        {
            case ActionSkills.None:

                break;
            case ActionSkills.SkillOne:
                SkillOne();
                break;
            case ActionSkills.SkillTwo:
                SkillTwo();
                break;
            case ActionSkills.SkillThree:
                SkillThree();
                break;
            case ActionSkills.SkillFour:
                SkillFour();
                break;
            case ActionSkills.SkillFive:
                SkillFive();
                break;
            default:
                break;
        }
    }



    void SkillOne()
    {

    }

    void SkillTwo()
    {

    }

    void SkillThree()
    {

    }

    void SkillFour()
    {

    }

    void SkillFive()
    {

    }

}

struct SkillInfo
{
    string skillName;
    Sprite skillSprite;
}
