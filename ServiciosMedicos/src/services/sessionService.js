const TOKEN_KEY = 'token'
const USER_KEY = 'user'

export function isAuthenticated() {
  return Boolean(sessionStorage.getItem(TOKEN_KEY))
}

export function getSessionUser() {
  try {
    const user = JSON.parse(sessionStorage.getItem(USER_KEY) ?? 'null')
    return user && typeof user === 'object' ? user : null
  } catch {
    return null
  }
}

export function clearSession() {
  sessionStorage.removeItem(TOKEN_KEY)
  sessionStorage.removeItem(USER_KEY)
}
