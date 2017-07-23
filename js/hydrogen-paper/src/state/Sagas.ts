import { SagaIterator } from 'redux-saga'
import { takeEvery, call, put, select } from 'redux-saga/effects'
import Snowflake, {
  Platform,
  Game,
  ConfigurationCollection
} from 'snowflake-remoting'

import * as Actions from './Actions'
import {
  successDispatch,
  failedDispatch,
  syncDispatch,
  SyncPayload
} from './ResultDispatches'

import * as Selectors from './Selectors'

function* refreshPlatformsWorker (snowflake: Snowflake): SagaIterator {
  try {
    const platforms: { [platformId: string]: Platform } = yield call(snowflake.stone.getPlatforms)
    yield put(successDispatch(Actions.refreshPlatforms, platforms))
  } catch (e) {
    yield put(failedDispatch(Actions.refreshPlatforms, e))
  }
}

function* refreshGamesWorker (snowflake: Snowflake): SagaIterator {
  try {
    const games: { [gameGuid: string]: Game } = yield call(snowflake.games.getGames)
    yield put(successDispatch(Actions.refreshGames, games))
  } catch (e) {
    yield put(failedDispatch(Actions.refreshGames, e))
  }
}

function* refreshActiveState (): SagaIterator {
  const query: { [key: string]: string } = yield select(Selectors.queryParamsSelector)
  yield put(syncDispatch(Actions.setActivePlatform, query['platform']))
  yield put(syncDispatch(Actions.setActiveGame, query['game']))
  // yield put(syncDispatch(Actions.refreshActiveGameConfiguration.started, { emulatorName: 'TestEmulator', gameUuid: query['game'] }))
}

/*
function* refreshActiveGameConfiguration (snowflake: Snowflake, action: SyncPayload<{emulatorName: string, gameUuid: string}>): SagaIterator {
  try {
    const config: ConfigurationCollection = yield call(snowflake.emulators.getConfiguration, action.payload.emulatorName, action.payload.gameUuid)
    yield put(successDispatch(Actions.refreshActiveGameConfiguration, config))
  } catch (e) {
    yield put(failedDispatch(Actions.refreshActiveGameConfiguration, e))
  }
}*/

function* retrieveGameConfigurations (snowflake: Snowflake, action: SyncPayload<{gameGuid: string, profileName: string}>): SagaIterator {
  try {
    const { gameGuid, profileName } = action.payload
    const configs: { [emulatorName: string]: ConfigurationCollection } = yield call(snowflake.games.getConfigurations, gameGuid, profileName)
    yield put(successDispatch(Actions.retrieveGameConfiguration, configs, action.payload))
  } catch (e) {
    yield put (failedDispatch(Actions.retrieveGameConfiguration, e))
  }
}

function* rootSaga (snowflake: Snowflake): SagaIterator {
  yield takeEvery(Actions.refreshPlatforms.type, refreshPlatformsWorker, snowflake)
  yield takeEvery(Actions.refreshGames.type, refreshGamesWorker, snowflake)
  yield takeEvery(Actions.locationChange, refreshActiveState)
  // yield takeEvery(Actions.refreshActiveGameConfiguration.started, refreshActiveGameConfiguration, snowflake)
  yield takeEvery(Actions.retrieveGameConfiguration.started, retrieveGameConfigurations, snowflake)
}

export default rootSaga
