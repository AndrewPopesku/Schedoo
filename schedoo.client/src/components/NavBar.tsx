import { AppBar, Container, Toolbar, Typography, Box, IconButton, Menu, MenuItem, Button, Tooltip, Avatar } from "@mui/material";
import React from "react";
import { useAuth } from "../hooks/useAuth";
import { useLocation, useNavigate } from "react-router-dom";

export function NavBar() {
    const { auth, setAuth } = useAuth();
    const location = useLocation();
    const navigate = useNavigate();
    const [anchorElUser, setAnchorElUser] = React.useState<null | HTMLElement>(null);

    const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorElUser(event.currentTarget);
    };

    const handleCloseUserMenu = () => {
        setAnchorElUser(null);
    };

    const navigateTo = (target: string) => {
        navigate(target, { state: { from: location } })
    }

    return (
        <>
            <AppBar position="static" color="info">
                <Container>
                    <Toolbar disableGutters>
                        <Typography
                            variant="h5"
                            noWrap
                            component="a"
                            sx={{
                                mr: 2,
                                display: { xs: 'none', md: 'flex' },
                                fontFamily: 'monospace',
                                fontWeight: 700,
                                letterSpacing: '.2rem',
                                color: 'inherit',
                                textDecoration: 'none',
                            }}
                        >
                            Schedoo
                        </Typography>

                        <Box sx={{ flexGrow: 1, display: { xs: 'none', md: 'flex' } }}>
                            
                        </Box>
                        {auth?.email ?
                            <>
                                <Typography
                                    sx={{
                                        color: 'inherit',
                                        marginRight: 3
                                    }}
                                >
                                    {auth.email}
                                </Typography>
                                <Box sx={{ flexGrow: 0 }}>
                                    <Tooltip title="Open settings">
                                        <IconButton onClick={handleOpenUserMenu} sx={{ p: 0 }}>
                                            <Avatar alt="Remy Sharp" src="/static/images/avatar/2.jpg" />
                                        </IconButton>
                                    </Tooltip>
                                    <Menu
                                        sx={{ mt: '45px' }}
                                        id="menu-appbar"
                                        anchorEl={anchorElUser}
                                        anchorOrigin={{
                                            vertical: 'top',
                                            horizontal: 'right',
                                        }}
                                        keepMounted
                                        transformOrigin={{
                                            vertical: 'top',
                                            horizontal: 'right',
                                        }}
                                        open={Boolean(anchorElUser)}
                                        onClose={handleCloseUserMenu}
                                    >
                                        <MenuItem
                                            onClick={() => navigateTo("/profile")}
                                        >
                                            <Typography textAlign="center">
                                                Profile
                                            </Typography>
                                        </MenuItem>

                                        <MenuItem
                                            onClick={() => setAuth({})}
                                        >
                                            <Typography textAlign="center">
                                                Sign Out
                                            </Typography>
                                        </MenuItem>
                                    </Menu>
                                </Box>
                            </>
                            :
                            <Box sx={{ display: { xs: 'none', sm: 'block' } }}>
                                <Button
                                    sx={{ color: '#fff' }}
                                    onClick={() => navigateTo("/login")}
                                >
                                    Log in
                                </Button>
                                <Button
                                    sx={{ color: '#fff' }}
                                    onClick={() => navigateTo("/register")}
                                >
                                    Register
                                </Button>
                            </Box>
                        }
                    </Toolbar>
                </Container>
            </AppBar>
        </>
    )
}