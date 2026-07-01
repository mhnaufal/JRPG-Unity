using UnityEngine;

public class BattleAudio
{
    private readonly AudioSource _source;
    private readonly AudioClip _playerAttack;
    private readonly AudioClip _enemyAttack;
    private readonly AudioClip _playerDefense;
    private readonly AudioClip _playerSpecial;

    public BattleAudio(AudioSource source, AudioClip playerAttack, AudioClip enemyAttack, AudioClip playerDefense, AudioClip playerSpecial)
    {
        _source = source;
        _playerAttack = playerAttack;
        _enemyAttack = enemyAttack;
        _playerDefense = playerDefense;
        _playerSpecial = playerSpecial;
    }

    public void PlayPlayerAttack() => _source.PlayOneShot(_playerAttack);
    public void PlayEnemyAttack() => _source.PlayOneShot(_enemyAttack);
    public void PlayPlayerDefense() => _source.PlayOneShot(_playerDefense);
    public void PlayPlayerSpecial() => _source.PlayOneShot(_playerSpecial);
}
