using System.Diagnostics;

namespace BulletMLLib
{
  /// <summary>
  /// This task pauses for a specified amount of time before resuming
  /// </summary>
  public class TriggerTask : BulletMLTask
  {
    #region Members

    /// <summary>
    /// String identifying the event
    /// </summary>
    private string Flag { get; set; }

    #endregion //Members

    #region Methods

    /// <summary>
    /// Initializes a new instance of the <see cref="BulletMLLib.BulletMLTask"/> class.
    /// </summary>
    /// <param name="node">Node.</param>
    /// <param name="owner">Owner.</param>
    public TriggerTask(TriggerNode node, BulletMLTask owner)
      : base(node, owner)
    {
      System.Diagnostics.Debug.Assert(null != Node);
      System.Diagnostics.Debug.Assert(null != Owner);
    }

    /// <summary>
    /// this sets up the task to be run.
    /// </summary>
    /// <param name="bullet">Bullet.</param>
    protected override void SetupTask(Bullet bullet)
    {
      Flag = Node.Text;
    }

    /// <summary>
    /// Run this task and all subtasks against a bullet
    /// This is called once a frame during runtime.
    /// </summary>
    /// <returns>ERunStatus: whether this task is done, paused, or still running</returns>
    /// <param name="bullet">The bullet to update this task against.</param>
    public override ERunStatus Run(Bullet bullet)
    {
      bullet.MyBulletManager.Trigger(bullet, Flag);

      TaskFinished = true;
      return ERunStatus.End;
    }

    #endregion //Methods
  }
}