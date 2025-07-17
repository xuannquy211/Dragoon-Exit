using UnityEngine;

public class RainAbnormality : Abnormality
{
    [SerializeField] private Transform enviroment;
    [SerializeField] private GameObject holder;
    [SerializeField] private ParticleSystem particle;
    
    public override void Active()
    {
        holder.gameObject.SetActive(true);
        holder.transform.SetParent(EnvironmentManager.Instance.GetPlayer());
        holder.transform.localPosition = Vector3.zero;
        holder.transform.localRotation = Quaternion.identity;
        particle.Play();
    }

    public override void Deactive()
    {
        particle.Stop();
        UnityMainThread.Instance.AddDelayAction(2f, () =>
        {
            holder.transform.SetParent(enviroment);
            holder.SetActive(false);
        });
    }
}