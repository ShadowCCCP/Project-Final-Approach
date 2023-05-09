using GXPEngine; // For GameObject

public class CollisionInfo {
	public readonly Vec2 normal;
	public readonly GameObject other;
	public readonly float timeOfImpact;
	public readonly Vec2 velCOM = new Vec2(0, 0);

	public CollisionInfo(Vec2 pNormal, GameObject pOther, float pTimeOfImpact, Vec2 pVelCOM) {
		normal = pNormal;
		other = pOther;
		timeOfImpact = pTimeOfImpact;
		velCOM = pVelCOM;
	}

    public CollisionInfo(Vec2 pNormal, GameObject pOther, float pTimeOfImpact)
    {
        normal = pNormal;
        other = pOther;
        timeOfImpact = pTimeOfImpact;
    }
}
