from tensorforce.environments import Environment
from tensorforce.agents import Agent
from tensorforce.execution import Runner
import numpy as np
import requests


def getStateFromEnvironment(environment):
    return environment.get('state')


class PaladinServerEnvironment(Environment):

    def __init__(self):
        self.availableActions = []
        super().__init__()

    def states(self):
        return dict(type='int', shape=(2, 2), num_values=2*2)

    def actions(self):
        url = 'http://host.docker.internal:5000/actions'
        self.availableActions = requests.get(url).json()

        return dict(type='int', num_values=len(self.availableActions))

    # Optional: should only be defined if environment has a natural fixed
    # maximum episode length; otherwise specify maximum number of training
    # timesteps via Environment.create(..., max_episode_timesteps=???)
    def max_episode_timesteps(self):
        return super().max_episode_timesteps()

    # Optional additional steps to close environment
    def close(self):
        url = 'http://host.docker.internal:5000/environment/close'
        requests.post(url)
        super().close()

    def reset(self):
        url = 'http://host.docker.internal:5000/environment/reset'
        requests.post(url)
        return self.state()

    def execute(self, actions):
        url = 'http://host.docker.internal:5000/environment/action'
        action = self.availableActions[actions]
        environment = requests.post(url, json=action)

        next_state = getStateFromEnvironment(environment)
        terminal = np.random.random() < 0.5
        reward = np.random.random()
        return next_state, terminal, reward

    def state(self):
        url = 'http://host.docker.internal:5000/environment'
        environment = requests.get(url).json()
        return getStateFromEnvironment(environment)


environment = Environment.create(
    environment=PaladinServerEnvironment
)

agent = Agent.create(
    agent='tensorforce',
    environment=environment,
    update=64,
    optimizer=dict(optimizer='adam', learning_rate=1e-3),
    objective='policy_gradient',
    reward_estimation=dict(horizon=20)
)

runner = Runner(
    agent=agent,
    environment=environment
)

runner.run(num_episodes=200)

runner.run(num_episodes=100, evaluation=True)

runner.close()
